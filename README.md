## About Custom.Radzen.Blazor
This is extended version of the Radzen.Blazor library with custom functions added and original functionality preserved

Currently following components have been extended(how to use - see RadzenBlazorDemos project):
 - GoogleMap (custom icons for every marker, custom draggable cursor icon, rendering fixed)

Added static src/href links for js, css resources

## Get started

### 1. Install

Radzen Blazor Components are distributed as a [Radzen.Blazor nuget package](https://www.nuget.org/packages/Custom.Radzen.Blazor). You can add them to your project in one of the following ways
- Install the package from command line by running `dotnet add package Custom.Radzen.Blazor`
- Add the project from the Visual Nuget Package Manager
- Manually edit the .csproj file and add a project reference

### 2. Import the namespace

Open the `_Imports.razor` file of your Blazor application and add this line `@using Radzen.Blazor`.

### 3. Include a theme
Radzen Blazor components come with five free themes: Material, Standard, Default, Dark, Software and Humanistic.

To use a theme
1. Pick a theme. The [online demos](https://blazor.radzen.com/colors) allow you to preview the available options via the theme dropdown located in the header. The Material theme is currently selected by default.
1. Include the theme CSS file in your Blazor application. Open `Pages\_Layout.cshtml` (Blazor Server .NET 6+), `Pages\_Host.cshtml` (Blazor Server before .NET 6) or `wwwroot/index.html` (Blazor WebAssembly) and include the CSS file of  a theme CSS file by adding this snippet
   ```html
   <link rel="stylesheet" href="@RadzenResources.CssMaterialBase">
   ```

To include a different theme (i.e. Standard) just change the name of the CSS file:
```html
<link rel="stylesheet" href="@RadzenResources.CssStandartBase">
```

### 4. Include Radzen.Blazor.js

Open `Pages\_Layout.cshtml` (Blazor Server .NET 6+), `Pages\_Host.cshtml` (Blazor Server before .NET 6) or `wwwroot/index.html` (Blazor WebAssembly) and include this snippet:

```html
<script src="@RadzenResources.JsContent"></script>
```

### 5. Use a component
Use any Radzen Blazor component by typing its tag name in a Blazor page e.g.
```html
<RadzenButton Text="Hi"></RadzenButton>
```

#### Data-binding a property
```razor
<RadzenButton Text=@text />
<RadzenTextBox @bind-Value=@text />
@code {
  string text = "Hi";
}
```

#### Handing events

```razor
<RadzenButton Click="@ButtonClicked" Text="Hi"></RadzenButton>
@code {
  void ButtonClicked()
  {

  }
}
```

---

## About original Radzen.Blazor

![Radzen Blazor Components](RadzenBlazorDemos/wwwroot/images/radzen-blazor-components.png)

<h1 align="center">
    Radzen Blazor Components
</h1>

<p align="center">
    A set of <strong>70+ free and open source</strong> native Blazor UI controls.
</p>

<div align="center">

[See Online Demos](https://blazor.radzen.com) or [Read the Docs](https://blazor.radzen.com/docs/)

</div>
