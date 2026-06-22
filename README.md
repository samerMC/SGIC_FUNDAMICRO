# Sistema de Gestión de Clientes - Prueba Técnica FUNDAMICRO

## Descripción general

Proyecto que corresponde a una prueba técnica desarrollada en ASP.NET Web Forms con VB.NET, utilizando SQL Server como motor de base de datos.

El sistema permite gestionar clientes, autenticar usuarios y registrar en una bitácora las acciones realizadas sobre los clientes.

## Funcionalidades principales

- Inicio de sesión con usuario y contraseña.
- Validación de usuarios contra SQL Server.
- Contraseñas almacenadas mediante hash y salt.
- Manejo de sesión.
- Cierre de sesión.
- Protección de páginas internas.
- Mantenimiento de usuarios.
- Roles de usuario: Administrador y Operador.
- Mantenimiento de clientes.
- Creación, edición y eliminación lógica de clientes.
- Registro automático de acciones en bitácora.
- Consulta de bitácoras con filtros.
- Validaciones de formularios.
- Consultas SQL parametrizadas.
- Interfaz web ordenada y responsive.

## Tecnologías utilizadas

- ASP.NET Web Forms
- VB.NET
- .NET Framework 4.8
- SQL Server
- SQL Server Management Studio
- HTML
- CSS
- Bootstrap
- JavaScript
- Git
- GitHub

## Requisitos para ejecutar el proyecto

Para ejecutar el sistema se necesita:

- Visual Studio 2022 Community o superior.
- .NET Framework 4.8.
- SQL Server o SQL Server Express.
- SQL Server Management Studio.
- Navegador web moderno.
- Git, para clonar el repositorio.

## Estructura general del proyecto


Datos/
    Clases de acceso a datos hacia SQL Server.

Modelos/
    Clases que representan entidades del sistema.

Seguridad/
    Clases relacionadas con autenticación, sesión y contraseñas.

Utilidades/
    Clases auxiliares para validaciones y mensajes del sistema.

ScriptsSQL/
    Scripts para creación y actualización de la base de datos.

Content/
    Hojas de estilo del sistema.

Scripts/
    Archivos JavaScript y dependencias front-end.

Login.aspx
    Pantalla de inicio de sesión.

Inicio.aspx
    Pantalla principal del sistema.

Usuarios.aspx
    Mantenimiento de usuarios.

Clientes.aspx
    Mantenimiento de clientes.

Bitacoras.aspx
    Consulta de bitácoras de acciones.

Logout.aspx
    Cierre de sesión.

Web.config
    Configuración general y cadena de conexión.

## EXTRAS
Así mismo si no se quiere ejecutar los 2 scripts que tiene el proyecto alojados en SCriptsSQL, puedes hacer backup de la base de datos utilizada
anexo dicho backup en este repositorio.

## AYUDA SOBRE LA CADENA DE CONEXION EN SQL SERVER
## Cambio de cadena de conexión

El sistema utiliza una cadena de conexión definida en el archivo Web.config para conectarse a SQL Server.

La cadena de conexión se encuentra dentro del bloque:

<connectionStrings>
  <add name="ConexionDB"
       connectionString="Data Source=DESKTOP-UB4AC9Q\SQLEXPRESS;Initial Catalog=DB_SGIC_FUNDAMICRO;Integrated Security=True;TrustServerCertificate=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>

Si el proyecto se ejecuta en otro equipo, es posible que el nombre del servidor SQL Server sea diferente. En ese caso, se debe modificar únicamente el valor de `Data Source`.

Ejemplo:

Data Source=DESKTOP-UB4AC9Q\SQLEXPRESS;

Debe reemplazarse por la instancia de SQL Server correspondiente al equipo donde se ejecutará el sistema.

Algunos ejemplos comunes son:

Data Source=.;
Data Source=.\SQLEXPRESS;
Data Source=localhost;
Data Source=(localdb)\MSSQLLocalDB;
Data Source=NOMBRE_EQUIPO\SQLEXPRESS;

La propiedad Initial Catalog debe contener el nombre de la base de datos creada en SQL Server:

Initial Catalog=DB_SGIC_FUNDAMICRO;

Si el nombre de la base de datos se mantiene igual, no es necesario modificar esta parte.

### Ejemplo usando SQL Server Express

<connectionStrings>
  <add name="ConexionDB"
       connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=DB_SGIC_FUNDAMICRO;Integrated Security=True;TrustServerCertificate=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>

### Ejemplo usando LocalDB

<connectionStrings>
  <add name="ConexionDB"
       connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB_SGIC_FUNDAMICRO;Integrated Security=True;TrustServerCertificate=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>

### Ejemplo usando autenticación SQL Server

Si se utiliza autenticación con usuario y contraseña de SQL Server, la cadena puede configurarse así:

<connectionStrings>
  <add name="ConexionDB"
       connectionString="Data Source=SERVIDOR_SQL;Initial Catalog=DB_SGIC_FUNDAMICRO;User ID=USUARIO_SQL;Password=CONTRASENA_SQL;TrustServerCertificate=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>

En ese caso, se debe reemplazar:

SERVIDOR_SQL
USUARIO_SQL
CONTRASENA_SQL

por los datos reales del servidor.

Por seguridad, se recomienda no subir contraseñas reales al repositorio si se utiliza autenticación SQL Server.

### Cómo saber el nombre del servidor en SSMS

Para identificar el servidor correcto, abrir SQL Server Management Studio y revisar el campo Server name al momento de conectarse.

También se puede ejecutar la siguiente consulta:

SELECT @@SERVERNAME AS NombreServidor;

El resultado puede utilizarse como referencia para configurar el valor de Data Source en Web.config.

Después de modificar la cadena de conexión, se debe guardar el archivo Web.config, compilar nuevamente la solución y ejecutar el sistema para verificar que la conexión funcione correctamente.

