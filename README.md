# Sis457_Ferreteria

# Descripción del Negocio
Este proyecto es una aplicación de escritorio desarrollada en C# para la gestión integral de una ferretería. Permite el control de inventarios, gestión de clientes y la automatización del proceso de ventas.

# Arquitectura de la Solución
Siguiendo las buenas prácticas de la materia, la solución se divide en:
- CadFerreteria: Capa de Acceso a Datos (Entity Framework).
- ClnFerreteria: Capa Lógica de Negocios.
- CpFerreteria: Capa de Presentación (Windows Forms).

# Entidades Principales
(Tablas y Entidades)
Categoria: (id, nombre, usuarioRegistro, fechaRegistro, estado)

SubCategoria: (id, idCategoria, nombre, usuarioRegistro, fechaRegistro, estado)

Marca: (id, nombre, usuarioRegistro, fechaRegistro, estado)

UnidadMedida: (id, nombre, usuarioRegistro, fechaRegistro, estado)

Producto: (id, idSubCategoria, idUnidadMedida, idMarca, codigo, descripcion, precioVenta, stock, usuarioRegistro, fechaRegistro, estado)

Proveedor: (id, nit, razonSocial, representante, celular, usuarioRegistro, fechaRegistro, estado)

Cliente: (id, nit, razonSocial, telefono, usuarioRegistro, fechaRegistro, estado)

Empleado: (id, cedulaIdentidad, nombres, primerApellido, segundoApellido, fechaNacimiento, direccion, celular, cargo, usuarioRegistro, fechaRegistro, estado)

Usuario: (id, idEmpleado, usuario, clave, rol, usuarioRegistro, fechaRegistro, estado)

Compra: (id, idProveedor, fecha, total, usuarioRegistro, fechaRegistro, estado)

CompraDetalle: (id, idCompra, idProducto, cantidad, precioCompra, usuarioRegistro, fechaRegistro, estado)

Venta: (id, fecha, idUsuario, idCliente, total, usuarioRegistro, fechaRegistro, estado)

VentaDetalle: (id, idVenta, idProducto, cantidad, precioUnitario, usuarioRegistro, fechaRegistro, estado)
