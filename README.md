
# SmartBreadcrumbs

![Image](https://i.imgur.com/zLgObJh.png)

SmartBreadcrumbs is a .NET Core utility library for asp.net-core websites to easily add and customize breadcrumbs. It is easy to setup and even easier to use!

There is also a blog post that shows how to use this library here: [Managing breadcrumbs in ASP.NET core using SmartBreadcrumbs](https://blog.zhaytam.com/2018/06/24/asp-net-core-using-smartbreadcrumbs/).

## How it works

At initialization, SmartBreadcrumbs will create a node hierarchy using all the breadcrumb attributes used on Actions. You'll then be able to use a TagHelper that will check if the current action/controller has a node and will render the breadcrumb element if found.  
By default, SmartBreadcrumbs uses Bootstrap 4's classes to style the breadcrumbs.

## Install

SmartBreadcrumbs is available on Nuget:  
`Install-Package SmartBreadcrumbs`

## Usage

### Initialization

```csharp
services.UseBreadcrumbs(GetType().Assembly);
```

### Options

You can also specify some options when initializing SmartBreadcrumbs:
```csharp
services.UseBreadcrumbs(GetType().Assembly, options =>
{
	options.TagName = "nav";
	options.TagClasses = "";
	options.OlClasses = "breadcrumb";
	options.LiClasses = "breadcrumb-item";
	options.ActiveLiClasses = "breadcrumb-item active";
	options.SeparatorElement = "<li class=\"separator\">/</li>";
});
```
The SeperatorElement will be added after each node, use it when you're using a custom theme/template for example.

### Adding nodes

#### Using the Breadcrumb attribute:

```csharp
[DefaultBreadcrumb("My home")]
public IActionResult Index()
{
	// We set the Home.Index as the default node
	// "My home" is the title that the node will use
	return View();
}

[Breadcrumb("ViewData.Title", FromAction = "Home.Index")]
[Breadcrumb("ViewData.Title", CacheTitle = true, FromAction = "Home.Index")]
public IActionResult Action1()
{
	// Here we set Home.Action1 to be a child of Home.Index by using
	// the FromAction property (Controller.Action)
	ViewData["Title"] = "Action 1";
	
	// The title in this case is "ViewData.Title", SmartBreadcrumbs
	// will then look for "Title" in the ViewData and use it
	// as a title for the node.

	// The CatchTitle option will replace the node's title with the
	// value of ViewData.Title, so use it when needed.
	return View();
}
```

FYI: You'll receive an error if you don't set a default node or if there is more than one default node.

#### Manually setting the current nodes

In some cases , you would like to set the current nodes yourself. For example, you want the titles of some nodes to be taken from the database or something.  
Luckily, SmartBreadcrumbs can trust you on this:
```csharp
public IActionResult Action2()
{
	// Manually create the nodes (assuming you used the attribute
	// to create a Default node, otherwise create it manually too).
	var childNode1 = new BreadcrumbNode("Action 1", "Home", "Action1", null, new { id = 10 });
	
	// When manually creating nodes, you have the option to use
	// route values in case you need them
	var childNode2 = new BreadcrumbNode("Action 2", "Home", "Action2", childNode1);
	
	// All you have to do now is tell SmartBreadcrumbs about this
	ViewData["BreadcrumbNode"] = childNode2; // Use the last node
	
	return View();
}
```

### Rendering using the TagHelper

The last step is to make use of the taghelper to render the breadcrumbs:  
`<breadcrumb></breadcrumb`  
Don't forget to import it in `_ViewImports.cshtml`:  
`@addTagHelper *, SmartBreadcrumbs`

### Result

```html
<nav>
	<ol class="breadcrumb">
		<li class="breadcrumb-item"><a href="/">Home</a></li>
		<li class="breadcrumb-item"><a href="/Action1?id=10">Action 1</a></li>
		<li class="breadcrumb-item active">Action 2</li>
	</ol>
</nav>
```

## More customization?

Since SmartBreadcrumbs initializes the nodes hiearchy in the beginning, you have the ability to not use the `breadcrumb` TagHelper and just create yours.  
You'll have access to a `BreadcrumbsManager` in your services that has the nodes. You can use them to create your own TagHelper for example.

## ChangeLog

**1.0.0**  
Initial release

**1.0.1**
- SmartBreadcrumbs can now handle controllers that don't have direct inheritance of Controller (Thanks to denis-pujdak-adm-it).
- Other type of IActionResult are also handled (Thanks to denis-pujdak-adm-it).

**1.0.2**
- Added option to cache node titles coming from the ViewData (see [Using the Breadcrumb attribute](https://github.com/zHaytam/SmartBreadcrumbs#using-the-breadcrumb-attribute)) (Thanks to Tronhus).

**1.0.3**
- Added TagClasses and SeparatorElement properties, see the [Options](https://github.com/zHaytam/SmartBreadcrumbs#options) section.

**1.0.3.2**
- Fixed the missing IActionContextAccessor error (using TryAddScoped the service).

**1.0.3.3**
- Fix IActionContextAccessor DI register (Thanks to askalione).

**1.0.5**
- Added AreaName support.
- Added IconClasses support.

## Credits & License

Credits: zHaytam  
License: SmartBreadcrumbs is open source, licensed under the [MIT License](https://github.com/zHaytam/SmartBreadcrumbs/blob/master/LICENSE).
