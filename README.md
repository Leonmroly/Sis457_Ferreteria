# Sis457_Ferreteria

# Descripción del Negocio
Este proyecto es una aplicación de escritorio desarrollada en C# para la gestión integral de una ferretería. Permite el control de inventarios, gestión de clientes y la automatización del proceso de ventas.

# Arquitectura de la Solución
Siguiendo las buenas prácticas de la materia, la solución se divide en:
- CadFerreteria: Capa de Acceso a Datos (Entity Framework).
- ClnFerreteria: Capa Lógica de Negocios.
- CpFerreteria: Capa de Presentación (Windows Forms).

# Entidades Principales
1. Entidades de Catálogo (Organización)
Categoría: Clasificación macro (Pinturas, Herramientas).

SubCategoría: Clasificación específica (Martillos, Cables).

Marca: Fabricante del producto (Truper, Bosch).

UnidadMedida: Cómo se despacha (Metros, Kilos, Piezas).

2. Entidad Central (El Objeto)
Producto: Une todas las anteriores. Contiene el código, la descripción técnica, el stock y el precio de venta.

3. Entidades Operativas (Movimientos)
Compra / CompraDetalle: Entrada de mercadería y relación con el Proveedor.

Venta / VentaDetalle: Salida de mercadería, relación con el Cliente y el Usuario.

4. Entidades de Personal (Seguridad)
Empleado: Datos humanos del trabajador.

Usuario: La cuenta con la que el empleado entra al sistema (Login, Clave, Rol).

Proveedor: Datos de la empresa que nos surte.

Cliente: Datos para la facturación.
