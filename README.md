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

## Builds
- develop [![Build Status](https://dev.azure.com/sovize/CrossBind/_apis/build/status/CrossBind?branchName=develop)](https://dev.azure.com/sovize/CrossBind/_build/latest?definitionId=2&branchName=develop)
- beta    [![Build Status](https://dev.azure.com/sovize/CrossBind/_apis/build/status/CrossBind?branchName=release%2Fv0.1.0)](https://dev.azure.com/sovize/CrossBind/_build/latest?definitionId=2&branchName=release%2F*)
- master  [![Build Status](https://dev.azure.com/sovize/CrossBind/_apis/build/status/CrossBind?branchName=master)](https://dev.azure.com/sovize/CrossBind/_build/latest?definitionId=2&branchName=master)


## Deployments
<table>
    <tr>
        <th>RELEASE</th>
        <th><a href="CrossBind.Engine/README.md">CrossBind Engine</a></th>
        <th><a href="CrossBind.Compiler/README.md">CrossBind Compiler</a></th>
        <th><a href="CrossBind/README.md">CrossBind CLI</a></th>
        <th><a href="CrossBind.Lang/README.md">CrossBind LSP</a></th>
    </tr>
    <tr>
        <th>Canary</th>
        <td>
            <img src="https://vsrm.dev.azure.com/sovize/_apis/public/Release/badge/df07a3c2-4bca-419c-a1d5-1af6bc9cc1b8/1/1" alt="deployment"/>
        </td>
        <td>
            <img src="https://vsrm.dev.azure.com/sovize/_apis/public/Release/badge/df07a3c2-4bca-419c-a1d5-1af6bc9cc1b8/1/1" alt="deployment"/>
        </td>
        <td>
            <img src="https://vsrm.dev.azure.com/sovize/_apis/public/Release/badge/df07a3c2-4bca-419c-a1d5-1af6bc9cc1b8/1/1" alt="deployment"/>
        </td>
        <td>
            <img src="https://vsrm.dev.azure.com/sovize/_apis/public/Release/badge/df07a3c2-4bca-419c-a1d5-1af6bc9cc1b8/1/1" alt="deployment"/>
        </td>
    </tr>
    <tr>
        <th>Beta</th>
        <td>
            <img src="https://vsrm.dev.azure.com/sovize/_apis/public/Release/badge/df07a3c2-4bca-419c-a1d5-1af6bc9cc1b8/2/4" alt="deployment"/>
        </td>
        <td>
            <img src="https://vsrm.dev.azure.com/sovize/_apis/public/Release/badge/df07a3c2-4bca-419c-a1d5-1af6bc9cc1b8/2/4" alt="deployment"/>
        </td>
        <td>
            <img src="https://vsrm.dev.azure.com/sovize/_apis/public/Release/badge/df07a3c2-4bca-419c-a1d5-1af6bc9cc1b8/2/3" alt="deployment"/>
        </td>
        <td>
            <img src="https://vsrm.dev.azure.com/sovize/_apis/public/Release/badge/df07a3c2-4bca-419c-a1d5-1af6bc9cc1b8/2/3" alt="deployment"/>
        </td>
    </tr>
    <tr>
        <th>Prod</th>
        <td>None</td>
        <td>None</td>
        <td>None</td>
        <td>None</td>
    </tr>
</table>

# :link: Links

- [Official Website](https://crossbind.dev/)
- [Plugin documentation](https://crossbind.dev/docs/beta/)

## Downloads
- __Canary__
    - *CLI*
        - [Linux x64](http://crossbind.dev/bin/latest/CrossBind)
        - [Windows x64](http://crossbind.dev/bin/latest/CrossBind.exe)
    - *LSP*
        - [Linux x64](http://crossbind.dev/bin/latest/CrossBind.Lang)
        - [Windows x64](http://crossbind.dev/bin/latest/CrossBind.Lang.exe)

# License

CrossBind general license is distributed under [Apache 2.0 license](https://www.apache.org/licenses/LICENSE-2.0.html)

All direct dependencies of the project are under [MIT license](https://mit-license.org/)