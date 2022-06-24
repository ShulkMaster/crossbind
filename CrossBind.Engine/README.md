# CrossBind Engine

Project containing the definitions of the CMU and the interface that both the CLI and the
plugins implement to interoperate correctly. It also has some utility classes such as
CSS processing and metadata extraction so that the plugins can generate code
as accurate as possible.

Once the porject is added to your dependencies make sure to add the following
lines to your `csproj` file.

```msbuild
<ItemGroup>
    <PackageReference Include="CrossBind.Engine" Version="x.y.z">
      <Private>false</Private>
      <ExcludeAssets>runtime</ExcludeAssets>
    </PackageReference>
    /// ... other dependencies
</ItemGroup>
```
This isa required for the plugin to work because the CLI already has the
assembly `CrossBind.Engine` loaded onto memory so by excluding it we avoid
runtime dependencies conflicts

also it is required to especify to MSBUILD that this assembly will support
dynamic loading so add the following

```msbuild
<PropertyGroup>
    /// ... other props
    <EnableDynamicLoading>true</EnableDynamicLoading>
</PropertyGroup>
```

Now your assembly will be able to dynamically load by the CLI just make
sure that the version of the CLI you intent to run the plugin with has a major and minor
version equal to the `CrossBind.Engine` assembly version. New feature may come
into the compiler and might not be fully backwards compatible.

Path version on the other and are guaranteed to work with older version of the plugins
since must commonly those are only bug fix releases.