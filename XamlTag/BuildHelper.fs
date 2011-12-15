namespace XamlTag
open System;

type X()=
  static member Nest(func : Func<IXamlBuilder,XamlCreator>)=
    func
  static member NestMany(func : Func<IXamlBuilder,XamlCreator[]>)=
    func

