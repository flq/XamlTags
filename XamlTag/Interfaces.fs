namespace XamlTag

open System

type XamlCreator=
  abstract member GetXamlObject : unit->Object

type IXamlBuilder=
  abstract member Start<'a> : unit -> [<return: System.Runtime.CompilerServices.DynamicAttribute>] Object
  abstract member Start<'a> : Object -> [<return: System.Runtime.CompilerServices.DynamicAttribute>] Object