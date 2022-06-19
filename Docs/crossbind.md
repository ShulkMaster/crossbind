# CrossBind Docs

---

Los archivos HBT cumplen con la función de ser una **unidad de traducción,** concepto nacido en
C y el cual representa una unidad básica de compilación. Consiste en el contenido de un único archivo fuente y
adicionalmente el contenido de otras unidades de traduccion que son conosidas como dependencias.

En este caso, los archivos escritos con el lenguaje *Haibt* y extensión HBT tendrán las secciones:

## Componentes

---

```php
Component ComponentName() extends Component{
...	
...
}
```

Donde `Component` **f**unciona tanto como palabra reservada para definir un componente y como la clase extendible la cual es la raíz de la jerarquía de clases.
Cada clase tiene a `Component` como superclase (e.g `Button`, `Select`, `TextInput`, etc).

En el caso del nombramiento de clases y propiedades el lenguaje soporta los naming conventions de `camelCase`, `PascalCase`, `snake_case`.

## Propiedades

---

```php
Component ComponentName() extends Component{
...	
	prop MyProp : int = 15;
...
}
```

Donde `prop` es la palabra reservada para declarar una propiedad. Seguido del mismo deben de ir dos puntos (`:`) y posterior a ellos el tipo de dato que se quiere asignar, signo igual (`=`) y finalmente el valor deseado. El lenguaje soporta los siguientes tipos de datos:

| Tipo | Descripción |
| --- | --- |
| int | Primitivo |
| double | Primitivo |
| string | Primitivo |
| boolean  | Primitivo |

Tambien es posible que una variable o propiedad pueda tener un valor por defecto, en estos casos la declaración de las mismas deben ir seguidas con un signo de interrogación (`?`) al lado del nombre, así:

```php
Component ComponentName() extends Component{
...	
	prop MyProp? : int = 15;
...
}
```

## Variantes

---

Las variantes son objetos que representan un mismo control pero con diseños y/o comportamientos diferentes, como los bordes y colores de un botón, etc.

Las variates poseen la siguiente estructura:

```php
Component ComponentName() extends Component{
...	
  variant myVariant;
  myVariant::primary{
    background: red;
    padding: 2px;
  }
...
}
```

Donde `variant`  sirve como palabra reservada para declararlas, seguido del nombre. Posterior a su declaración,
las variantes pueden ser manipuladas en el código colocando el nombre, dos dos puntos (`::`) y el nombre del valor  al que se le van a modificar los estilos.

Al igual que como sucede en CSS, la **especificidad** juega un papel importante a la hora de elegir cuales estilos aplicar si existe alguna colisión entre valores.

Cabe mencionar que si se da el caso de que exista algún estilo compartido en todo el componente, estos pueden ser declarados dentro del mismo:

```php
Component ComponentName() extends Component{
...	
  variant myVariant;
  padding: 2px;
  myVariant::primary{
    background: red;
  }
...
}
```

En estos casos donde los estilos estan afuera de alguna variante, internamente estos serán agrupados y aplicados en una sola clase.

## Markup

---

Por defecto, al momento de crear un componente y extender de otro, este tendrá el maquetado HTML que posea el padre, sin embargo, es posible sobreescribirlo y colocar markup extra:

```php
Component ComponentName() extends Button{
...	
  variant myVariant;
  padding: 2px;
  myVariant::primary{
    background: red;
  }
  <div>
    <p> Hello World </p>
    <base>
  </div>
...
}
```

Donde `<base>` será el código HTML que posee por herencia el componente heredado.
