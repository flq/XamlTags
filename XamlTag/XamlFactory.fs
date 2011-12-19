namespace XamlTag

open System
open System.Windows
open System.Windows.Markup

type XamlFactory<'a>(xamlObject : 'a->'a)=
  let cons = xamlObject
  interface IXamlFactory<'a> with
    member this.Create dataContext=
      let fw = box (Activator.CreateInstance<'a>()) :?> FrameworkElement
      fw.DataContext <- dataContext
      box fw :?> 'a |> xamlObject