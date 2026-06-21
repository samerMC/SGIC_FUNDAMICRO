/* ============================================================
   Script: Script_02_Actualizar_Usuarios.sql
   Base de datos: DB_SGIC_FUNDAMICRO
   Descripción:
   Agrega soporte para roles y mantenimiento de usuarios.
   ============================================================ */

USE DB_SGIC_FUNDAMICRO;
GO

/* ============================================================
   Tabla: Roles
   ============================================================ */

IF OBJECT_ID('dbo.Roles', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles
    (
        IdRol INT IDENTITY(1,1) NOT NULL,
        NombreRol NVARCHAR(50) NOT NULL,
        Descripcion NVARCHAR(150) NULL,
        Estado BIT NOT NULL CONSTRAINT DF_Roles_Estado DEFAULT (1),
        FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Roles_FechaCreacion DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_Roles PRIMARY KEY (IdRol),
        CONSTRAINT UQ_Roles_NombreRol UNIQUE (NombreRol)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE NombreRol = 'Administrador')
BEGIN
    INSERT INTO dbo.Roles (NombreRol, Descripcion)
    VALUES ('Administrador', 'Usuario con acceso a la administración del sistema.');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE NombreRol = 'Operador')
BEGIN
    INSERT INTO dbo.Roles (NombreRol, Descripcion)
    VALUES ('Operador', 'Usuario con acceso operativo al sistema.');
END
GO

/* ============================================================
   Mejoras a tabla Usuarios
   ============================================================ */

IF COL_LENGTH('dbo.Usuarios', 'IdRol') IS NULL
BEGIN
    ALTER TABLE dbo.Usuarios ADD IdRol INT NULL;
END
GO

IF COL_LENGTH('dbo.Usuarios', 'Correo') IS NULL
BEGIN
    ALTER TABLE dbo.Usuarios ADD Correo NVARCHAR(150) NULL;
END
GO

IF COL_LENGTH('dbo.Usuarios', 'FechaActualizacion') IS NULL
BEGIN
    ALTER TABLE dbo.Usuarios ADD FechaActualizacion DATETIME2(0) NULL;
END
GO

IF COL_LENGTH('dbo.Usuarios', 'IdUsuarioCreacion') IS NULL
BEGIN
    ALTER TABLE dbo.Usuarios ADD IdUsuarioCreacion INT NULL;
END
GO

IF COL_LENGTH('dbo.Usuarios', 'IdUsuarioActualizacion') IS NULL
BEGIN
    ALTER TABLE dbo.Usuarios ADD IdUsuarioActualizacion INT NULL;
END
GO

/* Asignar rol administrador al usuario admin */
UPDATE U
SET U.IdRol = R.IdRol
FROM dbo.Usuarios U
INNER JOIN dbo.Roles R ON R.NombreRol = 'Administrador'
WHERE U.NombreUsuario = 'admin'
  AND U.IdRol IS NULL;
GO

/* Asignar rol operador al resto de usuarios sin rol */
UPDATE U
SET U.IdRol = R.IdRol
FROM dbo.Usuarios U
INNER JOIN dbo.Roles R ON R.NombreRol = 'Operador'
WHERE U.IdRol IS NULL;
GO

/* Convertir IdRol en obligatorio cuando ya no existan valores nulos */
IF EXISTS
(
    SELECT 1 
    FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Usuarios')
      AND name = 'IdRol'
      AND is_nullable = 1
)
AND NOT EXISTS
(
    SELECT 1 FROM dbo.Usuarios WHERE IdRol IS NULL
)
BEGIN
    ALTER TABLE dbo.Usuarios ALTER COLUMN IdRol INT NOT NULL;
END
GO

/* Crear llave foránea entre Usuarios y Roles */
IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Usuarios_Roles'
)
BEGIN
    ALTER TABLE dbo.Usuarios
    ADD CONSTRAINT FK_Usuarios_Roles
        FOREIGN KEY (IdRol)
        REFERENCES dbo.Roles(IdRol);
END
GO

/* Índice único para correo, permitiendo valores NULL */
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'UX_Usuarios_Correo'
      AND object_id = OBJECT_ID('dbo.Usuarios')
)
BEGIN
    CREATE UNIQUE INDEX UX_Usuarios_Correo
    ON dbo.Usuarios(Correo)
    WHERE Correo IS NOT NULL;
END
GO

/* ============================================================
   Verificación
   ============================================================ */

SELECT 
    U.IdUsuario,
    U.NombreUsuario,
    U.NombreCompleto,
    U.Correo,
    R.NombreRol,
    U.Estado,
    U.FechaCreacion,
    U.FechaUltimoAcceso
FROM dbo.Usuarios U
INNER JOIN dbo.Roles R ON R.IdRol = U.IdRol
ORDER BY U.IdUsuario;

SELECT 
    IdRol,
    NombreRol,
    Estado
FROM dbo.Roles
ORDER BY IdRol;
GO