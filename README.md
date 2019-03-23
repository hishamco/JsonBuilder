# JsonBuilder
AppVeyor: [![Build status](https://ci.appveyor.com/api/projects/status/vvt7xurho6fkyhch?svg=true)](https://ci.appveyor.com/project/hishamco/jsonbuilder)

## Overview

`JsonBuilder` is a small library that demystify the creation of JSON objects in .NET.

## Features

- Lightweight
- Simplicity
- Support .NET & .NET Core frmeworks

## Usage

- [Creating JSON Objects](#creating-json-objects)
- [Adding Properties](#adding-properties)
- [JSON Formatting](#json-formatting)
- [Validating JSON Content](#validating-json-content)
- [Merge JSON Objects](#merge-json-objects)

### Creating JSON Objects
Creating JSON objects using `JsonBuilder` is quite simple
```csharp
var obj = new JsonBuilder();
```

### Adding Properties
`JsonBuilder` lets you to add properties as much as you want using `AppendProperty()` method.
```csharp
var obj = new JsonBuilder();
obj.AppendProperty("name", "Jon");
obj.AppendProperty("age", 22);
```
The actual result of the above JSON object is:
```json
{
    "name: "Jon",
    "age": 22
}
```

### JSON Formatting
`JsonBuilder` provides two types of formats:
1. Indent
This is the default format that format the JSON content with indentation in mind.
```json
{
    "name: "Jon",
    "age": 22
}
```
2. Minified
This is use minification to reduce the size o the JSON content
```json
{"name":"Jon","age":22}
```

### Validating JSON Content
`JsonBuilder` provides a `static` method called `IsValidJson()` to validate JSON content.
```csharp
var isValid = JsonBuilder.IsValidJson("{{"); // false

var isValid = JsonBuilder.IsValidJson("{name: \"Jon\"}"); // true
```

### Merge JSON Objects
`JsonBuilder` provides a `static` method called `MergeJsonObject()` to merge multiple JSON objects into a single JSON object.
```csharp
var json1 = "{\"name\":\"Jon\"}";
var json2 = "{\"name\":\"Doe\"}";
var jsonResult = JsonBuilder.MargeJsonObjects(JsonFormat.Indent, json1, json2);
```
The actual result of the above JSON object is:
```json
{
    "name: "Jon",
    "name": "Doe"
}
```