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

CREATE TABLE Cliente ( -- TABLA 5
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nit BIGINT NOT NULL,
  razonSocial VARCHAR(100) NOT NULL,
  telefono BIGINT NULL);

CREATE TABLE Proveedor ( -- TABLA 6
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nit BIGINT NOT NULL,
  razonSocial VARCHAR(100) NOT NULL UNIQUE,
  representante VARCHAR(50) NOT NULL,
  celular BIGINT NULL);

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

-- Proveedor
ALTER TABLE Proveedor ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Proveedor ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Proveedor ADD estado INT NOT NULL DEFAULT 1;

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






-- 1. PROCEDIMIENTO ALMACENADO (ESTILO MINERVA ADAPTADO A FERRETERÍA)
DROP PROC IF EXISTS paProductoListar;
GO
CREATE PROC paProductoListar @parametro VARCHAR(50)
AS
  SELECT p.id, p.idSubCategoria, p.idUnidadMedida, p.idMarca, p.codigo, p.descripcion, 
         um.nombre AS unidadMedida, m.nombre AS marca, p.saldo, p.precioVenta, 
         p.usuarioRegistro, p.fechaRegistro, p.estado
  FROM Producto p
  INNER JOIN UnidadMedida um ON p.idUnidadMedida = um.id
  INNER JOIN Marca m ON p.idMarca = m.id
  WHERE p.estado = 1 
    AND (p.codigo + p.descripcion + um.nombre + m.nombre) LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
  ORDER BY p.descripcion;
GO

-- 2. DML: DATOS DE PRUEBA (CORREGIDO Y AMPLIADO)
GO
USE LabFerreteria;
GO

-- Limpiar tablas antes de insertar (opcional, por si acaso)
-- DELETE FROM VentaDetalle; DELETE FROM Venta; DELETE FROM Producto; ...

-- Unidades de Medida (Cuidado: en Minerva era 'descripcion', aquí es 'nombre')
INSERT INTO UnidadMedida (nombre) VALUES ('Unidad'), ('Kilo'), ('Metro'), ('Caja'), ('Docena'), ('Paquete');

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

-- 3. PRUEBA DEL PROCEDIMIENTO
EXEC paProductoListar 'martillo';
EXEC paProductoListar 'stanley';

-- CONSULTAS FINALES
SELECT * FROM Producto;
SELECT * FROM Usuario;
SELECT * FROM Empleado;



ALTER PROC paProductoListar @parametro VARCHAR(50)
AS
BEGIN
  SELECT 
    p.id, 
    p.idSubCategoria,
    p.idUnidadMedida, 
    p.idMarca,        -- El ID que tienes en tu tabla Producto
    p.codigo, 
    p.descripcion, 
    um.nombre AS unidadMedida, -- Para que no de error de Unidad
    m.nombre AS marca,         -- ESTO ARREGLA EL ERROR DE MARCA (El nombre real)
    p.saldo,                   
    p.saldo AS stock,          -- Para que C# no llore
    p.precioVenta, 
    p.usuarioRegistro, 
    p.fechaRegistro, 
    p.estado
  FROM Producto p
  INNER JOIN UnidadMedida um ON um.id = p.idUnidadMedida
  INNER JOIN Marca m ON m.id = p.idMarca -- UNIÓN CON MARCA
  WHERE p.estado = 1 
    AND (p.codigo + p.descripcion + um.nombre + m.nombre LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
  ORDER BY p.descripcion;
END
GO