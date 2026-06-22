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
