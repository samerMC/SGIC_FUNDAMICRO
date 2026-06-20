/* ============================================================
   Base de datos: DB_SGIC_FUNDAMICRO
   Proyecto: Prueba técnica ASP.NET Web Forms VB.NET
   Descripción:
   Script para crear la base de datos y las tablas principales:
   - Usuarios
   - Clientes
   - Bitacoras

   Nota:
   Las contraseñas no se almacenan en texto plano. Se guardan
   como hash SHA-256 junto con un salt.
   ============================================================ */

IF DB_ID('DB_SGIC_FUNDAMICRO') IS NULL
BEGIN
    CREATE DATABASE DB_SGIC_FUNDAMICRO;
END
GO

USE DB_SGIC_FUNDAMICRO;
GO

/* ============================================================
   Tabla: Usuarios
   Finalidad:
   Almacena los usuarios autorizados para ingresar al sistema.
   ============================================================ */

IF OBJECT_ID('dbo.Usuarios', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        IdUsuario INT IDENTITY(1,1) NOT NULL,
        NombreUsuario NVARCHAR(50) NOT NULL,
        NombreCompleto NVARCHAR(150) NOT NULL,
        PasswordHash NVARCHAR(64) NOT NULL,
        PasswordSalt NVARCHAR(100) NOT NULL,
        Estado BIT NOT NULL CONSTRAINT DF_Usuarios_Estado DEFAULT (1),
        FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Usuarios_FechaCreacion DEFAULT (SYSDATETIME()),
        FechaUltimoAcceso DATETIME2(0) NULL,

        CONSTRAINT PK_Usuarios PRIMARY KEY (IdUsuario),
        CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario)
    );
END
GO

/* ============================================================
   Tabla: Clientes
   Finalidad:
   Almacena la información de clientes gestionados por el sistema.
   Se utiliza borrado lógico mediante el campo Estado.
   ============================================================ */

IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        IdCliente INT IDENTITY(1,1) NOT NULL,
        Nombre NVARCHAR(100) NOT NULL,
        Apellido NVARCHAR(100) NOT NULL,
        Dui NVARCHAR(10) NULL,
        Nit NVARCHAR(20) NULL,
        Telefono NVARCHAR(20) NULL,
        Correo NVARCHAR(150) NULL,
        Direccion NVARCHAR(250) NULL,
        Estado BIT NOT NULL CONSTRAINT DF_Clientes_Estado DEFAULT (1),
        FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Clientes_FechaCreacion DEFAULT (SYSDATETIME()),
        FechaActualizacion DATETIME2(0) NULL,
        IdUsuarioCreacion INT NULL,
        IdUsuarioActualizacion INT NULL,

        CONSTRAINT PK_Clientes PRIMARY KEY (IdCliente),

        CONSTRAINT FK_Clientes_UsuarioCreacion
            FOREIGN KEY (IdUsuarioCreacion)
            REFERENCES dbo.Usuarios(IdUsuario),

        CONSTRAINT FK_Clientes_UsuarioActualizacion
            FOREIGN KEY (IdUsuarioActualizacion)
            REFERENCES dbo.Usuarios(IdUsuario)
    );
END
GO

/* Índice para evitar DUI duplicado en clientes activos */
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'UX_Clientes_Dui_Activo'
      AND object_id = OBJECT_ID('dbo.Clientes')
)
BEGIN
    CREATE UNIQUE INDEX UX_Clientes_Dui_Activo
    ON dbo.Clientes(Dui)
    WHERE Dui IS NOT NULL AND Estado = 1;
END
GO

/* ============================================================
   Tabla: Bitacoras
   Finalidad:
   Registra las acciones realizadas sobre los clientes.
   Acciones esperadas:
   - AGREGAR
   - EDITAR
   - ELIMINAR
   ============================================================ */

IF OBJECT_ID('dbo.Bitacoras', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bitacoras
    (
        IdBitacora INT IDENTITY(1,1) NOT NULL,
        Accion NVARCHAR(20) NOT NULL,
        IdCliente INT NOT NULL,
        IdUsuario INT NOT NULL,
        NombreUsuario NVARCHAR(50) NOT NULL,
        FechaHora DATETIME2(0) NOT NULL CONSTRAINT DF_Bitacoras_FechaHora DEFAULT (SYSDATETIME()),
        Detalle NVARCHAR(500) NULL,
        IpOrigen NVARCHAR(45) NULL,

        CONSTRAINT PK_Bitacoras PRIMARY KEY (IdBitacora),

        CONSTRAINT FK_Bitacoras_Clientes
            FOREIGN KEY (IdCliente)
            REFERENCES dbo.Clientes(IdCliente),

        CONSTRAINT FK_Bitacoras_Usuarios
            FOREIGN KEY (IdUsuario)
            REFERENCES dbo.Usuarios(IdUsuario),

        CONSTRAINT CK_Bitacoras_Accion
            CHECK (Accion IN ('AGREGAR', 'EDITAR', 'ELIMINAR'))
    );
END
GO

/* Índices para mejorar consultas de bitácora */
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Bitacoras_FechaHora'
      AND object_id = OBJECT_ID('dbo.Bitacoras')
)
BEGIN
    CREATE INDEX IX_Bitacoras_FechaHora
    ON dbo.Bitacoras(FechaHora DESC);
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Bitacoras_IdCliente'
      AND object_id = OBJECT_ID('dbo.Bitacoras')
)
BEGIN
    CREATE INDEX IX_Bitacoras_IdCliente
    ON dbo.Bitacoras(IdCliente);
END
GO

/* ============================================================
   Usuario administrador inicial
   Usuario: admin
   Contraseña de prueba: Admin@2025

   Hash generado con:
   SHA256(PasswordSalt + Contraseña)

   PasswordSalt:
   GSG_FUNDAMICRO_2025
   ============================================================ */

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Usuarios
    WHERE NombreUsuario = 'admin'
)
BEGIN
    INSERT INTO dbo.Usuarios
    (
        NombreUsuario,
        NombreCompleto,
        PasswordHash,
        PasswordSalt,
        Estado
    )
    VALUES
    (
        'admin',
        'Administrador del Sistema',
        'A5D15ACCD4E06787498A59E1A8F165CD12E6F97FF882B7A28F4464D40BC371A1',
        'GSG_FUNDAMICRO_2025',
        1
    );
END
GO

/* ============================================================
   Consultas de verificación
   ============================================================ */

SELECT 
    IdUsuario,
    NombreUsuario,
    NombreCompleto,
    Estado,
    FechaCreacion
FROM dbo.Usuarios;

SELECT 
    TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
GO