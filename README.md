# IPrint
Adds PrintDialog wraper to your project (with preview window)

Repo contains two projects:
* **IPrint** - Class library.
* **IPrintTest** - C# Sample Project, examine IPrint class library usage.

### IPrint class library usage:
#### C#

###### FlowDocument

```C#
using IPrint;

FlowDocument fd;

// Fill FlowDocument here 

  // prints document to default printer with default settings
IPrintDialog.PrintDocument(fd);

// OR

  // show preview Window with printer/page settings
IPrintDialog.PreviewDocument(fd);
```

###### UIElement

```C#
using IPrint;

Grid uie;

// Fill Grid here 

  // prints document to default printer with default settings
IPrintDialog.PrintUIElement(uie);

// OR

  // show preview Window with printer/page settings
IPrintDialog.PreviewUIElement(uie);
```

### Parameters:

* **flowdocument** - Document that will be printed
* **uielement** - UIElement that will be printed


