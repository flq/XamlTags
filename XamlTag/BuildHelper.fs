namespace XamlTag
open System;

type X()=
  static member N(func : Func<IXamlBuilder,XamlCreator>)=
    func
  static member NM(func : Func<IXamlBuilder,XamlCreator[]>)=
    func

