# SmartBreadcrumbs

![Image](https://i.imgur.com/zLgObJh.png)

SmartBreadcrumbs is a .NET Core utility library for asp.net-core websites to easily add and customize breadcrumbs. It is easy to setup and even easier to use!

## How it works

At initialisation, SmartBreadcrumbs will create a node hierachy using all the breadcrumb attributes used on Actions. You'll then be able to use a TagHelper that will check if the current action/controller has a node and will render the breadcrumb element if found.  
By default, SmartBreadcrumbs uses Bootstrap 4's classes to style the breadcrumbs.

## Install

SmartBreadcrumbs is available on Nuget:  
`Install-Package SmartBreadcrumbs`

## Usage

### Initialisation

```csharp
services.UseBreadcrumbs(GetType().Assembly);
```

### Options

You can also specify some options when initializing SmartBreadcrumbs:
```csharp
services.UseBreadcrumbs(GetType().Assembly, options =>
{
	TagName = "nav";
	OlClasses = "breadcrumb";
	LiClasses = "breadcrumb-item";
	ActiveLiClasses = "breadcrumb-item active";
});
```

### Adding nodes

#### Using the Breadcrumb attribute:

```csharp
[Breadcrumb("My home", DefaultNode = true)]
public IActionResult Index()
{
	// We set the Home.Index as the default node
	// "My home" is the title that the node will use
	return View();
}

[Breadcrumb("ViewData.Title", FromAction = "Home.Index")]
public IActionResult Action1()
{
	// Here we set Home.Action1 to be a child of Home.Index by using
	// the FromAction property (Controller.Action)
	ViewData["Title"] = "Action 1";
	// The title in this case is "ViewData.Title", SmartBreadcrumbs
	// will then look for "Title" in the ViewData and use it
	// as a title for the node.
	return View();
}
```

FYI: You'll receive an error if you don't set a default node or if there is more than one default node.

#### Manually setting the current nodes

In some cases , you would like to set the current nodes yourself. For example, you want the titles of some nodes to be taken from the database or something.  
Luckly, SmartBreadcrumbs can trust you on this:
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

## Credits & License

Credits: zHaytam  
License: SmartBreadcrumbs is open source, licensed under the [MIT License](https://github.com/zHaytam/SmartBreadcrumbs/blob/master/LICENSE).
