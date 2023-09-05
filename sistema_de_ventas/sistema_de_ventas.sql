
CREATE DATABASE db_sistema_venta
GO

USE db_sistema_venta
GO

CREATE TABLE Rol(
	id_rol int primary key identity(1,1),
	descripcion varchar(100),
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Permiso(
	id_permiso int primary key identity(1,1),
	id_rol int references Rol(id_rol),
	nombre_menu varchar(100),
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Proveedor(
	id_proveedor int primary key identity(1,1),
	documento varchar(50),
	razon_social varchar(100),
	correo varchar(100),
	telefono varchar(50),
	estado bit,
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Cliente(
	id_cliente int primary key identity(1,1),
	documento varchar(50),
	nombre_completo varchar(100),
	correo varchar(100),
	telefono varchar(50),
	estado bit,
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Usuario(
	id_usuario int primary key identity(1,1),
	id_rol int references Rol(id_rol),
	documento varchar(50),
	clave varchar(50),
	nombre_completo varchar(100),
	correo varchar(100),
	telefono varchar(50),
	estado bit,
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Categoria(
	id_categoria int primary key identity(1,1),
	descripcion varchar(100),
	estado bit,
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Producto(
	id_producto int primary key identity(1,1),
	id_categoria int references Categoria(id_categoria),
	codigo varchar(50),
	nombre varchar(100),
	descripcion  varchar(100),
	stock int not null default 0,
	precio_compra decimal(10,2) default 0,
	precio_venta decimal(10,2) default 0,
	estado bit,
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Compra(
	id_compra int primary key identity(1,1),
	id_usuario int references Usuario(id_usuario),
	id_proveedor int references Proveedor(id_proveedor),
	tipo_documento varchar(50),
	numero_documento varchar(50),
	monto_total decimal(10,2),
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Detalle_Compra(
	id_detalle_compra int primary key identity(1,1),
	id_compra int references Compra(id_compra),
	id_producto int references Producto(id_producto),
	precio_compra decimal(10,2) default 0,
	precio_venta decimal(10,2) default 0,
	cantidad int,
	monto_total decimal(10,2),
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Venta(
	id_venta int primary key identity(1,1),
	id_usuario int references Usuario(id_usuario),
	tipo_documento varchar(50),
	numero_documento varchar(50),
	documento_cliente varchar(50),
	nombre_cliente varchar(100),
	monto_pago decimal(10,2),
	monto_cambio decimal(10,2),
	monto_total decimal(10,2),
	fecha_registro datetime default getdate()
)
GO

CREATE TABLE Detalle_Venta(
	id_detalle_venta int primary key identity(1,1),
	id_venta int references Venta(id_venta),
	id_producto int references Producto(id_producto),
	precio_venta decimal(10,2),
	cantidad int,
	subtotal decimal(10,2),
	fecha_registro datetime default getdate()
)
GO