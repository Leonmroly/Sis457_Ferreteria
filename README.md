# Sis457_Ferreteria

# Descripción del Negocio
Este proyecto es una aplicación de escritorio desarrollada en C# para la gestión integral de una ferretería. Permite el control de inventarios, gestión de clientes y la automatización del proceso de ventas.

# Arquitectura de la Solución
Siguiendo las buenas prácticas de la materia, la solución se divide en:
- CadFerreteria: Capa de Acceso a Datos (Entity Framework).
- ClnFerreteria: Capa Lógica de Negocios.
- CpFerreteria: Capa de Presentación (Windows Forms).

# Entidades Principales
- Producto: (id, codigo, descripcion, precio, stock, registroActivo)
- Cliente: (id, nit, razonSocial, telefono, registroActivo)
- Usuario: (id, usuario, clave, rol, registroActivo)
- Venta: (id, fecha, idUsuario, idCliente, total, registroActivo)
- VentaDetalle: (id, idVenta, idProducto, cantidad, precioUnitario)
