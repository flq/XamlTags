namespace XamlTag

open System
open System.ComponentModel
open System.Reflection
open System.Windows
open System.Windows.Controls
open XamlModule

type ConverterKey = { Source : Type; Target : Type }

type internal ValueConverter()=    
    
    let knownConverters = new System.Collections.Generic.Dictionary<ConverterKey,_->_->_>()
    do
      knownConverters.Add({ Source = typeof<Object[]>; Target = typeof<UIElementCollection>}, 
        fun (values : Object[]) (z : Panel) -> values |> Seq.map (fun v -> v :?> UIElement) |> Seq.iter (fun v -> z.Children.Add v |> ignore))

    let convert (converter : TypeConverter option) value =
        match converter with
        | Some(converter) -> converter.ConvertFrom(null,System.Globalization.CultureInfo.InvariantCulture,value)
        | None -> failwith "dead"

    member x.GetValue<'a> propName value=
        let proptype = (typeof<'a>.GetPropertyType propName)
        if proptype.IsAssignableFrom(value.GetType()) then
          value
        else
          try
            convert (proptype.GetConverter()) value
          with
            | :? Exception -> sprintf "No converter found to convert %s to target type %s" (value.GetType().Name) proptype.Name |> invalidOp

