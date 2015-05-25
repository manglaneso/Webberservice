# Webberservice
Web service and client created with Visual Studio 2012 in C# for university.

It has a web method that receives an SVG file in form of a byte[] and returns the file converted to PNG using an external web service in another byte[] and rotated 270º. The client just opens the SVG file, transforms it to byte[] and calls the web service, once returned, the byte[] is transformed to file again and stored in the desired path.

This only works on Microsoft Windows systems.

**Authors**
Andrés Manglano Cañizares
Fayán Leonardo Pardo
