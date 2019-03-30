# SmartBreadcrumbs 2.0.0

A utility library for ASP.NET Core (both MVC and Razor Pages) websites to easily add and customize breadcrumbs.

## About the 2.0.0 version

When I created SmartBreadcrumbs, it only worked on MVC websites.  
Razor Pages then came out and became "more popular", so I rewrote the whole code base, cleaned the project's structure, wrote unit tests and now, SmartBreadcrumbs works on both MVC and Razor Pages websites, even on the same project.  
**If you're interested in the README of the old version, it's available here: [OLD_README](https://github.com/zHaytam/SmartBreadcrumbs/blob/master/OLD_README.md).**

## Informations

||Badges|
|--|--|
|Build| [![Build Status](https://dev.azure.com/haytamzanid0913/SmartBreadcrumbs/_apis/build/status/zHaytam.SmartBreadcrumbs?branchName=master)](https://dev.azure.com/haytamzanid0913/SmartBreadcrumbs/_build/latest?definitionId=1&branchName=master) ![Azure DevOps tests](https://img.shields.io/azure-devops/tests/haytamzanid0913/SmartBreadcrumbs/1.svg)|
|NuGet|[![NuGet](https://img.shields.io/nuget/v/SmartBreadcrumbs.svg)](https://www.nuget.org/packages/SmartBreadcrumbs) [![Nuget](https://img.shields.io/nuget/dt/SmartBreadcrumbs.svg)](https://www.nuget.org/packages/SmartBreadcrumbs)|
|License|[![GitHub](https://img.shields.io/github/license/zHaytam/SmartBreadcrumbs.svg)](https://github.com/zHaytam/SmartBreadcrumbs)|

## Documentation & Example

 - [Wiki](https://github.com/zHaytam/SmartBreadcrumbs/wiki): A list of all the possible things you can do with SmartBreadcrumbs.
 - [RazorPagesAndMvc](https://github.com/zHaytam/SmartBreadcrumbs/tree/master/examples/RazorPagesAndMvc): An example project containing both MVC and Razor Pages pages. Check it out to see how SmartBreadcrumbs works.

## Usage

### Install

Install the package using NuGet  
`Install-Package SmartBreadcrumbs`

### Initialize

After you have setup your breadcrumbs (using the `Breadcrumb` and `DefaultBreadcrumb` attributes), go ahead and add SmartBreadcrumbs in your services:  
```cs
services.AddBreadcrumbs(GetType().Assembly);
```

### Use

SmartBreadcrumbs comes with a `breadcrumb` tag that renders the breadcrumbs for you, to use it:

 1. In `_ViewImports.cshtml`, add `@addTagHelper *, SmartBreadcrumbs`.
 2. In your `_Layout.cshml` (or specific pages), use `<breadcrumb></breadcrumb>`.

### Customize

#### Options

You can specify options when you initialize SmartBreadcrumbs:  
```cs
services.AddBreadcrumbs(GetType().Assembly, options =>
{
	options.TagName = "nav";
	options.TagClasses = "";
	options.OlClasses = "breadcrumb";
	options.LiClasses = "breadcrumb-item";
	options.ActiveLiClasses = "breadcrumb-item active";
	options.SeparatorElement = "<li class=\"separator\">/</li>";
});
```

#### More

Check out the documentation if you need more customization!
 - You can manually create and set breadcrumb nodes, useful when you have dynamic breadcrumbs (e.g. E-Commerce website).
 - You can get access to the `BreadcrumbManager` and create your own `TagHelper` for example.

## Credits & License

Credits: zHaytam and the contributors.  
License: SmartBreadcrumbs is open source, licensed under the [MIT License](https://github.com/zHaytam/SmartBreadcrumbs/blob/master/LICENSE).
