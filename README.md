# BACKEND - C# .NET (API) 🚀

## Objetivo

_Desarrollar una API para ingresar peliculas/series/genero, la cual permitirá conocer y modificar los
personajes que lo componen y entender en qué películas estos participaron._
- Se Utilizo .NET Core 5 Web API.
- Se Utilizo DataAnnotations para el manejo de envio de datos a los endpoints.
- Se implemento el modelo CodeFirst para el modelado de datos y Entity Framework Core.
- Se implemento los patrones Repositorio y Unidad de trabajo (Repository and Unit Of Work Patterns).
- Se implemento las siguientes abstracciones: Controller->Handlers->UnitOfWork->Repository.

## Uso

1- Se debe cambiar las credenciales del correo electronico.
   Se dispuso de un usuario para probar los endpoints. Credenciales email=invitado@invitado.com, password=Invitado_1234, evitara los pasos 2 y 3.
   
2- Registrar usuario en endpoint "register" si se registro correctamente recibira "Revise su correo electronico para validar el registro".

3- Revise su correo donde recibira un enlace para validar el registro, al realizar un click lo redirigira a el servidor y tendra el siguiente mensaje "Su registro fue confirmado." 

4- Ingrese las credenciales al endpoint "login" donde recibira un token de 24hs de duracion.

5- Debera ingresar los distintos generos antes de poder ingresar personajes y/o peliculas/series.

## Requerimientos técnicos

### 1- Modelado de Base de Datos.

   * Personaje: tiene.
     * Imagen. (url)
     * Nombre.
     * Edad.
     * Peso.
     * Historia (Biografia).
     * Películas o series asociadas.
   * Película o Serie: tiene,
     * Imagen. (url)
     * Título.
     * Fecha de creación.
     * Calificación (del 1 al 5).
     * Personajes asociados.
   * Género: tiene,
     * Nombre.
     * Imagen. (url)
     * Películas o series asociadas.

### 2- Registro y Autenticación de Usuarios

   Para el registro se debe realizar registro al endpoint
   - api/v1/auth/register
     _Al registrarse se enviara un correo para validar el registro (se deberan cargar las credenciales en appsettings.json)
     Dicho email tendra validez por los proximos 10 minutos, caso contrario, debera volver a registrarse._
   
   - api/v1/auth/login _Endpoint que permite obtener el token con duracion de 24hs._

   - api/v1/auth/forgot _Endpoint que permite generar una nueva contraseña que debe ser modificada._

   - api/v1/auth/changePassword _Endpoint que permite cambiar la contraseña._

### 3- Listado de Personajes

El listado muestra:
* Imagen (url, ejemplo https://localhost:44357/characters/personaje.png).
* Nombre.

El endpoint es: 
* GET api/v1/characters

### 4- Creación, Edición y Eliminación de Personajes (CRUD)

Existen las operaciones básicas de creación, edición y eliminación de personajes, para dichas operaciones el usuario debe estar autorizado.
* POST api/v1/characters
* PUT  api/v1/characters/{id}
* DEL  api/v1/characters/{id}

### 5- Detalle de Personaje

En el detalle se listan todos los atributos del personaje, como así también sus películas o series relacionadas.
* GET  api/v1/characters/{id}

### 6- Búsqueda de Personajes

Permite buscar por nombre, filtrar por edad, peso o películas/series en las que participó.
Para especificar el término de búsqueda o filtros se especifican como parámetros de query:
* GET api/v1/characters?name=nombre
* GET api/v1/characters?age=edad
* GET api/v1/characters?movies=idMovie

### 7- Listado de Películas

El listado muestra:
* Imagen (url ejemplo https://localhost:44357/films/film.png).
* Titulo.
* Fecha de creación

El endpoint es: 
* GET api/v1/movies

### 8- Detalle de Película / Serie con sus personajes

Devuelve todos los campos de la película o serie junto a los personajes asociados a la misma.
* GET  api/v1/movies/{id}

### 9- Creación, Edición y Eliminación de Película / Serie (CRUD)

Existen las operaciones básicas de creación, edición y eliminación de películas o series, para dichas operaciones el usuario debe estar autorizado.
* POST api/v1/movies
* PUT  api/v1/movies/{id}
* DEL  api/v1/movies/{id}

### 10- Búsqueda de Películas o Series

Permite buscar por título, filtrar por género. Además, permite ordenar los resultados por fecha de creación de forma ascendiente o descendiente.
El término de búsqueda, filtro u ordenación se especifican como parámetros de query:
* GET api/v1/movies?name=nombre
* GET api/v1/movies?genre=idGenero
* GET api/v1/movies?order=ASC | DESC

### 11- Envío de emails

Al registrarse en el sitio, el usuario recibe un email de confirmacion de registro disponible por los proximos 10 minutos.
Ademas, permite reiniciar la contraseña el caso de olvidarla, para dicho caso recibira un correo con una nueva contraseña generada por un numero aleatorio de 8 digitos.
Luego de recibir la nueva contraseña, debera cambiarla por una nueva contraseña generada por el usuario.
Los endpoints son:
* GET  api/v1/Auth/confirm/{id}/{token}
* POST api/v1/Auth/forgot
* POST api/v1/Auth/changePassword
* POST api/v1/Auth/login
* POST api/v1/Auth/Register

## Pruebas

Se realizon un conjunto de test de los diferentes endpoints de la aplicacion, verificando las posibles respuestas de cada uno de ellos.

Para las pruebas se utilizaron xUnit y Moq.
