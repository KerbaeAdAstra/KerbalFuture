# Style Guide

## Naming

* Classes, enums, namespaces, and methods shall be named in `PascalCase`.
* Variables, fields, and properties shall be named in `camelCase`.
* Objects declared with either const or readonly shall be named with `UNDERSCORE_DELIMITED_ALLCAPS`.
* Names shall be descriptive of their purpose.
* Closely-related information should have names of the same length.

## Formatting

* There shall be no space between a method's name and its arguments.
* There shall be a space between a control structure (e.g. if, for) and its arguments.
* Class, method, property, control structure, et cetera bodies shall always be contained by curly brackets, even if they contain only one statement.
* Bracketed blocks shall begin the line after the control structure or declaration, e.g.,

```csharp
class Foo
{
    //…
}
```

* Indentation shall be one tab per rank.
* Lines shall have a length not exceeding 80 characters.
* Excessively long single lines shall be cut at logical points.

## Layout

* The order of members within a class shall be: static fields, instanced fields, static properties, instanced properties, constructors, methods.
* There shall be an empty line between: the last field and the first property; the last property and the first constructor; each pair of constructors; the last constructor and the first method; each pair of methods.
* There shall be one class, enum, or struct in each file.
* Instanced fields and properties shall be initialized in constructors, not in declaration.

## Commenting

* Single-line comments shall be on their own line, not appended to any other code.
* Single-line comments shall use the double-slash notation `//`, as opposed to the block-comment notation, `/* … */`.
* There shall be a block comment immediately preceding every method documenting said method. It should be documented in [this](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/how-to-use-the-xml-documentation-features) format.
* Comments within the body of a method shall precede the content in question and be preceded by an empty line themselves.
