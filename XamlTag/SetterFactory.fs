namespace XamlTag

open System
open System.ComponentModel
open System.Collections.ObjectModel
open System.Reflection
open System.Windows
open System.Windows.Controls
open System.Windows.Data
open XamlModule

type internal SetterFactory()=    
    
    let knownSetterProvider = new System.Collections.Generic.List<ISetterProvider>()
    do
      knownSetterProvider.Add(new StandardSetterProvider())
      knownSetterProvider.Add(new ArrayToListSetter())
      knownSetterProvider.Add(new ConverterBasedSetter())

    let convert (converter : TypeConverter option) value =
        match converter with
        | Some(converter) -> converter.ConvertFrom(null,System.Globalization.CultureInfo.InvariantCulture,value)
        | None -> failwith "dead"

    member private x.SetBindValue<'v> argName (args  : Object[]) (argNames : ReadOnlyCollection<String>) setter=
      let idx = argNames.IndexOf(argName)
      if not (idx = -1) then
        let v = args.[idx + 1] :?> 'v // First value is always the path without arg name, hence +1
        setter(v)

    member x.GetAction<'a> propName (value : Object)=
        let ctx = { PropertyName = propName; Value = value; PropertyType = (typeof<'a>.GetPropertyType propName) }
        try
          let setter = knownSetterProvider |> Seq.tryFind (fun provider -> provider.Match ctx)
          match setter with
          | Some(setter) -> setter.Setter<'a> ctx
          | None -> failwith "End of road"
        with
            | :? Exception -> raise(new NotSupportedException(sprintf "Could not resolve a way to set %s to target (%s:%s)" (value.GetType().Name) ctx.PropertyName ctx.PropertyType.Name))

    member x.GetBindAction<'a> depProp (args : Object[]) (argNames : ReadOnlyCollection<String>)=
        fun (a : 'a) ->
          let fw = box a :?> FrameworkElement
          let binding = new Binding(args.[0].ToString())
          x.SetBindValue<IValueConverter> "converter" args argNames (fun v -> binding.Converter <- v)
          fw.SetBinding(depProp, binding) |> ignore