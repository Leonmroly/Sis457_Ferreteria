-- DDL
CREATE DATABASE LabFerreteria;
GO
USE master
GO
CREATE LOGIN usrferreteria WITH PASSWORD = '123456',
  DEFAULT_DATABASE = LabFerreteria,
  CHECK_EXPIRATION = OFF,
  CHECK_POLICY = ON
GO
USE LabFerreteria
GO
CREATE USER usrferreteria FOR LOGIN usrferreteria
GO
ALTER ROLE db_owner ADD MEMBER usrferreteria
GO

-- 1. LIMPIEZA TOTAL (Orden jerárquico para evitar errores de FK)
DROP TABLE IF EXISTS MovimientoInventario;
DROP TABLE IF EXISTS VentaDetalle;
DROP TABLE IF EXISTS Venta;
DROP TABLE IF EXISTS CompraDetalle;
DROP TABLE IF EXISTS Compra;
DROP TABLE IF EXISTS Producto;
DROP TABLE IF EXISTS SubCategoria;
DROP TABLE IF EXISTS Categoria;
DROP TABLE IF EXISTS Marca;
DROP TABLE IF EXISTS UnidadMedida;
DROP TABLE IF EXISTS Cliente;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Empleado;
DROP TABLE IF EXISTS Proveedor;
GO

-- 2. TABLAS MAESTRAS (BASE)
CREATE TABLE UnidadMedida ( -- TABLA 1
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(20) NOT NULL);

CREATE TABLE Marca ( -- TABLA 2
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(50) NOT NULL);

CREATE TABLE Categoria ( -- TABLA 3
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(50) NOT NULL);

CREATE TABLE SubCategoria ( -- TABLA 4
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idCategoria INT NOT NULL,
  nombre VARCHAR(50) NOT NULL,
  CONSTRAINT fk_Sub_Cat FOREIGN KEY (idCategoria) REFERENCES Categoria(id));

  



-- Borramos la anterior para que no haya choques
IF OBJECT_ID('Cliente', 'U') IS NOT NULL DROP TABLE Cliente;
GO

CREATE TABLE Cliente (
    id INT PRIMARY KEY IDENTITY(1,1),
    cedulaIdentidad BIGINT NOT NULL DEFAULT 0,
    nombreCompleto VARCHAR(100) NOT NULL,
    direccion VARCHAR(250) NULL,
    telefono VARCHAR(20) NULL,
    email VARCHAR(100) NULL,
    password VARCHAR(250) NULL,

    tipo INT NOT NULL DEFAULT 1, 
    
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT (suser_name()),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado INT NOT NULL DEFAULT 1 
);
GO

SELECT 'Tabla Cliente con C.I. creada con éxito' AS Resultado;


SELECT 'Tabla Cliente creada con éxito' AS Resultado;



-- Creamos la relación con la tabla Venta (si ya tienes la tabla Venta creada)
ALTER TABLE Venta
ADD CONSTRAINT FK_Venta_Cliente
FOREIGN KEY (idCliente) REFERENCES Cliente(id);
GO


-- 1. Si existe una relación que nos bloquea, la buscamos y la matamos
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
    + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) 
    + ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.foreign_keys
WHERE referenced_object_id = OBJECT_ID('Proveedor');
EXEC sp_executesql @sql;

-- 2. Borramos la tabla vieja
IF OBJECT_ID('Proveedor', 'U') IS NOT NULL DROP TABLE Proveedor;

-- 3. Creamos la tabla definitiva
CREATE TABLE Proveedor (
    id INT PRIMARY KEY IDENTITY(1,1),
    nit BIGINT NOT NULL,
    razonSocial VARCHAR(100) NOT NULL,
    direccion VARCHAR(250) NULL,
    telefono VARCHAR(20) NULL,
    email VARCHAR(100) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT (suser_name()),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado INT NOT NULL DEFAULT 1 -- 1: Activo, -1: Eliminado
);

CREATE TABLE Empleado ( -- TABLA 7
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cedulaIdentidad VARCHAR(12) NOT NULL UNIQUE,
  nombres VARCHAR(50) NOT NULL,
  primerApellido VARCHAR(50) NULL,
  segundoApellido VARCHAR(50) NULL,
  fechaNacimiento DATE NOT NULL,
  direccion VARCHAR(250) NOT NULL,
  celular BIGINT NOT NULL,
  cargo VARCHAR(50) NOT NULL);

CREATE TABLE Usuario ( -- TABLA 8
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idEmpleado INT NOT NULL,
  usuario VARCHAR(15) NOT NULL UNIQUE,
  clave VARCHAR(250) NOT NULL,
  rol VARCHAR(20) NOT NULL,
  CONSTRAINT fk_Usu_Emp FOREIGN KEY (idEmpleado) REFERENCES Empleado(id));

-- 3. PRODUCTOS Y MOVIMIENTOS
CREATE TABLE Producto ( -- TABLA 9
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idSubCategoria INT NOT NULL,
  idUnidadMedida INT NOT NULL,
  idMarca INT NOT NULL,
  codigo VARCHAR(20) NOT NULL,
  descripcion VARCHAR(200) NOT NULL,
  saldo DECIMAL(18,2) NOT NULL DEFAULT 0,  
  precioVenta DECIMAL NOT NULL CHECK (precioVenta > 0), 
  CONSTRAINT fk_Prod_Sub FOREIGN KEY (idSubCategoria) REFERENCES SubCategoria(id),
  CONSTRAINT fk_Prod_UM FOREIGN KEY (idUnidadMedida) REFERENCES UnidadMedida(id),
  CONSTRAINT fk_Prod_Marca FOREIGN KEY (idMarca) REFERENCES Marca(id));

-- 4. TRANSACCIONES: COMPRAS Y VENTAS
CREATE TABLE Compra ( -- TABLA 10
  id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idProveedor INT NOT NULL,
  fecha DATETIME NOT NULL DEFAULT GETDATE(),
  total DECIMAL(18,2) NOT NULL,
  CONSTRAINT fk_Com_Prov FOREIGN KEY (idProveedor) REFERENCES Proveedor(id));

CREATE TABLE CompraDetalle ( -- TABLA 11
  id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idCompra BIGINT NOT NULL,
  idProducto INT NOT NULL,
  cantidad DECIMAL(18,2) NOT NULL,
  precioCompra DECIMAL(18,2) NOT NULL,
  CONSTRAINT fk_DetCom_Com FOREIGN KEY (idCompra) REFERENCES Compra(id),
  CONSTRAINT fk_DetCom_Prod FOREIGN KEY (idProducto) REFERENCES Producto(id));

CREATE TABLE Venta ( -- TABLA 12
  id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
  fecha DATETIME NOT NULL DEFAULT GETDATE(),
  idUsuario INT NOT NULL,
  idCliente INT NOT NULL,
  total DECIMAL(18,2) NOT NULL,
  CONSTRAINT fk_Ven_Usu FOREIGN KEY (idUsuario) REFERENCES Usuario(id),
  CONSTRAINT fk_Ven_Cli FOREIGN KEY (idCliente) REFERENCES Cliente(id));

CREATE TABLE VentaDetalle ( -- TABLA 13
  id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idVenta BIGINT NOT NULL,
  idProducto INT NOT NULL,
  cantidad DECIMAL(18,2) NOT NULL,
  precioUnitario DECIMAL(18,2) NOT NULL,
  CONSTRAINT fk_DetVen_Ven FOREIGN KEY (idVenta) REFERENCES Venta(id),
  CONSTRAINT fk_DetVen_Prod FOREIGN KEY (idProducto) REFERENCES Producto(id));

-- 5. AUDITORÍA (Estilo Minerva profesional)
-- 5. AUDITORÍA (ESTILO MINERVA: 3 líneas por tabla)

-- Categoria
ALTER TABLE Categoria ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Categoria ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Categoria ADD estado INT NOT NULL DEFAULT 1;

-- SubCategoria
ALTER TABLE SubCategoria ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE SubCategoria ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE SubCategoria ADD estado INT NOT NULL DEFAULT 1;

-- Marca
ALTER TABLE Marca ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Marca ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Marca ADD estado INT NOT NULL DEFAULT 1;

-- UnidadMedida
ALTER TABLE UnidadMedida ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE UnidadMedida ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE UnidadMedida ADD estado INT NOT NULL DEFAULT 1;

-- Producto
ALTER TABLE Producto ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Producto ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Producto ADD estado INT NOT NULL DEFAULT 1;

-- Cliente
ALTER TABLE Cliente ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Cliente ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Cliente ADD estado INT NOT NULL DEFAULT 1;

ALTER TABLE Cliente ADD password VARCHAR(250) NULL;
ALTER TABLE Cliente ADD tipo INT NOT NULL DEFAULT 1; -- 1: Caja, 2: Cuenta Propia




-- Proveedor
ALTER TABLE Proveedor ADD nit BIGINT NOT NULL DEFAULT 0;
ALTER TABLE Proveedor ADD razonSocial VARCHAR(100) NOT NULL DEFAULT 'SIN NOMBRE';
ALTER TABLE Proveedor ADD direccion VARCHAR(250) NULL;
ALTER TABLE Proveedor ADD telefono VARCHAR(20) NULL;
ALTER TABLE Proveedor ADD email VARCHAR(100) NULL;
GO
-- Y ahora la conexión mágica con Compras
ALTER TABLE Compra
ADD CONSTRAINT FK_Compra_Proveedor
FOREIGN KEY (idProveedor) REFERENCES Proveedor(id);
GO


-- Empleado
ALTER TABLE Empleado ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Empleado ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Empleado ADD estado INT NOT NULL DEFAULT 1;

-- Usuario
ALTER TABLE Usuario ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Usuario ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Usuario ADD estado INT NOT NULL DEFAULT 1;

-- Venta
ALTER TABLE Venta ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Venta ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Venta ADD estado INT NOT NULL DEFAULT 1;

-- VentaDetalle
ALTER TABLE VentaDetalle ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE VentaDetalle ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE VentaDetalle ADD estado INT NOT NULL DEFAULT 1;

-- Compra
ALTER TABLE Compra ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Compra ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Compra ADD estado INT NOT NULL DEFAULT 1;

-- CompraDetalle
ALTER TABLE CompraDetalle ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE CompraDetalle ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE CompraDetalle ADD estado INT NOT NULL DEFAULT 1;
GO





drop proc if exists paProductoListar;
-- 1. PROCEDIMIENTO ALMACENADO (ESTILO MINERVA ADAPTADO A FERRETERÍA)
-- Si el procedimiento ya existe, lo borramos para crearlo limpio
IF OBJECT_ID('paProductoListar', 'P') IS NOT NULL
    DROP PROC paProductoListar;
GO

CREATE PROC paProductoListar @parametro VARCHAR(50)
AS
BEGIN
  SELECT p.id, 
         p.idSubCategoria, -- Este es el nombre real en la tabla
         p.idUnidadMedida, 
         p.idMarca, 
         p.codigo, 
         p.descripcion, 
         um.nombre AS unidadMedida, 
         m.nombre AS marca, 
         c.nombre AS categoria, -- Aquí sacamos el nombre de la categoría
         p.saldo,               -- Usamos solo saldo para que no salga doble
         p.precioVenta, 
         p.usuarioRegistro, 
         p.fechaRegistro, 
         p.estado
  FROM Producto p
  INNER JOIN UnidadMedida um ON p.idUnidadMedida = um.id
  INNER JOIN Marca m ON p.idMarca = m.id
  INNER JOIN Categoria c ON p.idSubCategoria = c.id -- Unimos por el ID real
  WHERE p.estado = 1 
    AND (p.codigo + p.descripcion + um.nombre + m.nombre + c.nombre) LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
  ORDER BY p.descripcion;
END
GO



CREATE PROC paMarcaListar @parametro VARCHAR(50)
AS
  SELECT id, nombre, usuarioRegistro, fechaRegistro, estado
  FROM Marca
  WHERE estado <> -1 AND nombre LIKE '%' + REPLACE(@parametro, ' ', '%') + '%';
GO



CREATE PROC paCategoriaListar @parametro VARCHAR(50)
AS
  SELECT id, nombre, usuarioRegistro, fechaRegistro, estado
  FROM Categoria
  WHERE estado <> -1 AND nombre LIKE '%' + REPLACE(@parametro, ' ', '%') + '%';
GO



ALTER TABLE dbo.Producto NOCHECK CONSTRAINT fk_Prod_Sub;

-- 2. Limpiamos la tabla de SubCategorias por completo
DELETE FROM dbo.SubCategoria;

-- 3. Reiniciamos el contador de IDs a 0
DBCC CHECKIDENT ('dbo.SubCategoria', RESEED, 0);

-- 4. Insertamos las subcategorías espejo basadas en tus Categorías actuales
-- Esto garantiza que los IDs coincidan 1 a 1
INSERT INTO dbo.SubCategoria (idCategoria, nombre, usuarioRegistro, fechaRegistro, estado)
SELECT id, nombre, 'SISTEMA', GETDATE(), 1 FROM dbo.Categoria;

-- 5. REGLA DE ORO: Actualizamos los productos viejos para que apunten a los nuevos IDs
-- Como ahora son espejos, el idSubCategoria debe ser igual al idCategoria que quisiste poner
UPDATE dbo.Producto SET idSubCategoria = 1 WHERE idSubCategoria IS NULL OR idSubCategoria <= 0;

-- 6. Reactivamos la restricción (si hay errores aquí, es porque hay productos con IDs que no existen)
ALTER TABLE dbo.Producto WITH CHECK CHECK CONSTRAINT fk_Prod_Sub;
GO




-- 1. Si existe algo con ese nombre, lo borramos para empezar de cero
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'paUnidadMedidaListar')
    DROP PROC paUnidadMedidaListar;
GO

-- 2. Lo creamos limpio (SIN la columna símbolo)
CREATE PROC paUnidadMedidaListar @parametro VARCHAR(50)
AS
BEGIN
  SELECT id, nombre, usuarioRegistro, fechaRegistro, estado
  FROM UnidadMedida
  WHERE estado <> -1 
    AND nombre LIKE '%' + REPLACE(@parametro, ' ', '%') + '%';
END
GO




-- 2. DML: DATOS DE PRUEBA (CORREGIDO Y AMPLIADO)
GO
USE LabFerreteria;
GO


-- Unidades de Medida (Cuidado: en Minerva era 'descripcion', aquí es 'nombre')
INSERT INTO UnidadMedida (nombre) 
VALUES ('Unidad'), ('Kilo'), ('Metro'), ('Caja'), ('Docena'), ('Paquete');


-- Marcas
INSERT INTO Marca (nombre) VALUES ('Truper'), ('Stanley'), ('Bosch'), ('Tramontina');

-- Categorías y Subcategorías
INSERT INTO Categoria (nombre) VALUES ('Herramientas Manuales'), ('Material Eléctrico'), ('Pinturas');
INSERT INTO SubCategoria (idCategoria, nombre) VALUES (1, 'Martillos'), (1, 'Destornilladores'), (2, 'Cables');

-- Personal y Acceso (Asegúrate de que los campos coincidan con tu CREATE TABLE)
INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, segundoApellido, fechaNacimiento, direccion, celular, cargo)
VALUES ('123456', 'Juan', 'Perez', 'Sosa', '1990-01-01', 'Calle Loa 123', 71717171, 'Administrador');

INSERT INTO Usuario (idEmpleado, usuario, clave, rol)
VALUES (1, 'admin', 'i0hcoO/nssY6WOs9pOp5Xw==', 'Administrador');

-- Clientes
INSERT INTO Cliente (nit, razonSocial, telefono) VALUES (0, 'Cliente General', 0);

-- Productos (Usando las IDs generadas arriba)
-- Martillo (SubCat 1, UM 1, Marca 1)
INSERT INTO Producto (idSubCategoria, idUnidadMedida, idMarca, codigo, descripcion, precioVenta, saldo) 
VALUES (1, 1, 1, 'MART-01', 'Martillo de uña 16oz Pro', 45.00, 50);

-- Cable (SubCat 3, UM 3, Marca 3)
INSERT INTO Producto (idSubCategoria, idUnidadMedida, idMarca, codigo, descripcion, precioVenta, saldo) 
VALUES (3, 3, 3, 'CAB-001', 'Cable Eléctrico Nro 14', 12.50, 100);

-- Destornillador (SubCat 2, UM 1, Marca 2)
INSERT INTO Producto (idSubCategoria, idUnidadMedida, idMarca, codigo, descripcion, precioVenta, saldo) 
VALUES (2, 1, 2, 'DEST-02', 'Destornillador Phillips Stanley', 25.00, 30);
GO


INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, segundoApellido, fechaNacimiento, direccion, celular, cargo)
VALUES ('654321', 'Roly', 'Leon', 'Mamani', '2005-05-15', 'Avenida Segungo Vascones', 72727272, 'Administrador');

INSERT INTO Usuario (idEmpleado, usuario, clave, rol)
VALUES (1, 'lroly', 'i0hcoO/nssY6WOs9pOp5Xw==', 'Administrador');

SELECT * FROM Cliente 
WHERE email = @usuario AND password = @password AND tipo = 2;

SELECT usuario, clave, estado FROM Usuario;

-- 3. PRUEBA DEL PROCEDIMIENTO
EXEC paProductoListar 'martillo';
EXEC paProductoListar 'stanley';

-- CONSULTAS FINALES
SELECT * FROM Producto;
SELECT * FROM Usuario;
SELECT * FROM Empleado;
SELECT * FROM Categoria;
SELECT * FROM Marca;
SELECT * FROM UnidadMedida;
SELECT id, nombre FROM Categoria;
-- Verifica si esta tabla tiene los mismos IDs que 'Categoria'
SELECT * FROM SubCategoria;
SELECT * FROM Proveedor;
SELECT * FROM Cliente;




UPDATE SubCategoria 
SET estado = -1 
WHERE idCategoria NOT IN (SELECT id FROM Categoria WHERE estado = 1);





UPDATE SubCategoria SET estado = -1;
-- 2. Actualizamos o Insertamos las que SÍ están activas en Categoría
-- Esto busca por nombre para no duplicar, y si existe le pone estado 1
MERGE INTO SubCategoria AS Target
USING (SELECT id, nombre FROM Categoria WHERE estado = 1) AS Source
ON (Target.idCategoria = Source.id)
WHEN MATCHED THEN
    UPDATE SET Target.nombre = Source.nombre, Target.estado = 1
WHEN NOT MATCHED THEN
    INSERT (idCategoria, nombre, usuarioRegistro, fechaRegistro, estado)
    VALUES (Source.id, Source.nombre, 'SISTEMA', GETDATE(), 1);
-- 3. Limpieza de seguridad: cualquier cosa que no tenga un padre vivo, se va
UPDATE SubCategoria SET estado = -1 
WHERE idCategoria NOT IN (SELECT id FROM Categoria WHERE estado = 1);
GO


---------
ALTER DATABASE LabFerreteria SET MULTI_USER WITH ROLLBACK IMMEDIATE;




SELECT 
    blocking_session_id AS SesionBloqueadora, 
    session_id AS SesionBloqueada, 
    wait_type, 
    wait_time, 
    last_wait_type, 
    text AS QueryTexto
FROM sys.dm_exec_requests
CROSS APPLY sys.dm_exec_sql_text(sql_handle)
WHERE blocking_session_id <> 0;


EXEC sp_updatestats;
GO
-- Esto limpia la memoria caché del motor de SQL
DBCC FREEPROCCACHE;
DBCC DROPCLEANBUFFERS;
GO




-- Si no existe la Razón Social, la agregamos
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Proveedor') AND name = 'razonSocial')
    ALTER TABLE Proveedor ADD razonSocial VARCHAR(100) NOT NULL DEFAULT 'SIN NOMBRE';

-- Si no existe la Dirección, la agregamos
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Proveedor') AND name = 'direccion')
    ALTER TABLE Proveedor ADD direccion VARCHAR(250) NULL;

-- Si no existe el Teléfono, la agregamos
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Proveedor') AND name = 'telefono')
    ALTER TABLE Proveedor ADD telefono VARCHAR(20) NULL;

-- Si no existe el Email, la agregamos
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Proveedor') AND name = 'email')
    ALTER TABLE Proveedor ADD email VARCHAR(100) NULL;

GO
-- Verificamos cómo quedó
SELECT TOP 0 * FROM Proveedor;