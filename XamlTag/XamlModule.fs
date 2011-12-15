﻿module XamlModule

open System
open System.ComponentModel
open System.Reflection

// Dynamic property setter
let (?<-) (this : 'Source) (property : string) (value : 'Value) =
    this.GetType().GetProperty(property).SetValue(this, value, null)

// Split a string on 'And'
let splitOnAnd (s : string) = s.Split([|"And"|], StringSplitOptions.RemoveEmptyEntries)

// Checks whether some object is of type a
let is<'a> (obj : Object)=
  match obj with
  | :? 'a as obj -> true
  | _ -> false

type System.Type with
  member x.GetPropertyType name=
    let p = x.GetProperty(name)
    if p = null then
      raise(ArgumentException(sprintf "Type %s does not have a property named %s" x.Name name))
    p.PropertyType

type System.Type with
    member x.GetConverter() : TypeConverter option=
        let instantiateTypeConverter(att : Object)=
            let converterName = (att :?> TypeConverterAttribute).ConverterTypeName
            let instance = converterName |> Type.GetType |> Activator.CreateInstance
            instance :?> TypeConverter
        let att = x.GetCustomAttributes(true) |> Seq.tryFind is<TypeConverterAttribute>
        match att with
        | Some(att) -> Some(att |> instantiateTypeConverter)
        | _ -> None