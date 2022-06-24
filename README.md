# CrossBind

CrossBind is the transpiler for the DSL called Haibt taylor towards declarative
UI component declarations that can be retarget to other JS frameworks component
notations code. Currently supported official are Vue and React.

Having a single set of source files will hell to library authors to create their
simple functional components once and then port them easily to other popular JS frameworks.

CrossBind project not only has develop the Transpiler itself, we also provide
a ser of tools such a a CLI to interact with the compiler, load Third party or
official plugins that will let you target your code other that officially supported

This includes a VS code extension to help with productivity and documentation for the plugin API
if you need to retarget the Haibt code to other platforms but without dealing with the parsing
of the source files.

# Projects
The following projects are part of the CrossBind infrastructure:
 - [CrossBind Engine](./CrossBind.Engine/README.md)
 - [CrossBind Engine](./CrossBind.Compiler/README.md)

# :link: Links
 - [Official Website](https://crossbind.dev/)
 - [Plugin documentation](https://crossbind.dev/docs/beta/)

# License

CrossBind general license is distributed under [Apache 2.0 license](https://www.apache.org/licenses/LICENSE-2.0.html)

All direct dependencies of the project are under [MIT license](https://mit-license.org/)