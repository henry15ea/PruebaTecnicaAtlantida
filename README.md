# PruebaTecnicaAtlantida
Este repositorio sirve como base comprobable del desarrollo de prueba tecnica .


tecnologías utilizadas para el desarrollo:

•	.NET

•	SQL Server.

•	.NET Razor.

•	JQUERY.

•	Materialize CSS.

•	Swagger.

•	AutoMapper.

•	Dapper.

•	JWT


-------------------------------------------------- Workspace de PostMan -----------------------------------------------------------


https://www.postman.com/research-saganist-34434493 

-------------------------------------------------------API REST .--------------------------------------------------------------------

La API Rest esta desarrollada utilizando tecnologías .Net , C# ,MVC . basado en el proyecto Web API que ofrece Visual Studio .

La depuración de la API se ha hecho con Swagger y POSTMAN .


Configuración :


Para configurar la conexión que tiene la API o el Proyecto Web API con la base de datos , esta se realiza en el Archivo llamado “ConectionDB.cs” que se puede encontrar en 

/tools/ConectionDB.cs

Una vez localizado el archivo “ConectionDB.cs” se procede a modificar la cadena de conexión relacionada a la instancia que se esta manejando en el sistema Operativo o en una instancia SQL Server alojada a un servicio externo como Azure DB.

Se modificara la cadena de conexión en la variable connectionString , tal como se muestra en la captura siguiente .

Realizando esto ya habrá configurado el proyecto para la conexión con la instancia que se tenga .


-----------------------------------------------------Cliente--------------------------------------------------------------------------------------- 

El cliente ha sido desarrollado utilizando también tecnologías de Microsoft .Net , Razor,Materialize CSS , JQUERY . dando una interfaz inspirada en colores ,logos del banco Atlantida . 


Configuración 

El proyecto también cuenta con una configuración la diferencia es que esta vez configuraremos la dirección y el puerto donde la API esta escuchando las peticiones, para ello haremos los siguientes pasos .

El el proyecto buscaremos la carpeta “tools” esto con el fin de encontrar el archivo llamado “RoutesApi.cs” que es el encargado de manejar las rutas o EndPoint que tiene la Api , como también configurar la dirección y puerto del servicio .


Para ello se nos mostrara como la pantalla siguiente , donde el proyecto llamado “PortalAtlantida” será el cliente. 


Una vez localizado el archivo procedemos a buscar la variable “HostApi” que contiene la dirección y puerto donde esta escuchando la API , la modificaremos según sea nuestro caso , si estamos utilizando otro puerto o desplegado en un servidor con dominio .


Donde solamente se modificara la dirección y puerto de la variable “HostApi” es la única configuración que será necesaria .


Nota : Tomar en cuenta que en la variable HostApi se configura de la siguiente manera : 

https://mi _servidor:puerto_de_escucha/api

Una vez que hemos configurado y que el proyecto de la API este corriendo en segundo plano , podemos correr el cliente . obteniendo las siguientes pantallas .




