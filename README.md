# IPrint
Adds PrintDialog wraper to your project (with preview window)

Repo contains two projects:
* **IPrint** - Class library.
* **IPrintTest** - C# Sample Project, examine IPrint class library usage.

### IPrint class library usage:
#### C#

```C#
using IPrint;

FlowDocument fd;

// Fill fd here 

  // prints document to default printer with default settings
IPrintProvider.PrintDocument(fd, "FlowDocument print test");

// OR

  // show preview Window with printer/page settings
IPrintProvider.ShowPreview(fd, "FlowDocument print test");
```

### Parameters:

* **flowdocument** - Document that will be printed

