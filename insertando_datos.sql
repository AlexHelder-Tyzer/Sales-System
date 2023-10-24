/*

INSERT INTO Rol (descripcion)
VALUES('ADMINISTRADOR')
INSERT INTO Rol (descripcion)
VALUES('EMPLEADO')

INSERT INTO Usuario (id_rol,documento, clave, nombre_completo, correo, telefono, estado)
VALUES(2, '73473648', '123456', 'Juan Gonzales', 'juan@gmail.com', '993875762', 1)

INSERT INTO Usuario(id_rol, documento, clave, nombre_completo, correo, telefono, estado)
VALUES(1, 'mila', '123456', 'Rosa Milagros Hilario', 'rosa.milagro@gmial.com', '996387578', 1)

*/


/* CONSULTA PARA MOSTRAR LOS PERMISOS DE UN USUARIO*/
select P.id_rol, P.nombre_menu from Permiso P
INNER JOIN Rol R ON R.id_rol = P.id_rol
INNER JOIN Usuario U ON U.id_rol = R.id_rol
WHERE U.id_usuario = 2

select * from Rol
select * from Permiso
select * from Usuario
/*
INSERT INTO Permiso (id_rol, nombre_menu)
VALUES(1,'menuUsuarios'),
(1,'menuMantenedor'),
(1,'menuVentas'),
(1,'menuCompras'),
(1,'menuUClientes'),
(1,'menuProveedores'),
(1,'menuReportes'),
(1,'menuAcercaDe')

INSERT INTO Permiso (id_rol, nombre_menu) VALUES
(2,'menuVentas'),
(2,'menuCompras'),
(2,'menuUClientes'),
(2,'menuProveedores'),
(2,'menuAcercaDe')

select U.id_usuario, U.documento, U.clave, U.nombre_completo, U.correo, U.telefono, U.estado,R.id_rol, R.descripcion
FROM Usuario U
INNER JOIN Rol R on R.id_rol = U.id_rol*/


/* CREAR PROCEDIMIENTO ALMACENADO REGISTRAR USUARIO*/
CREATE PROCEDURE SP_RegistrarUsuario(
	@id_rol int,
	@documento varchar(50),
	@clave varchar(50),
	@nombre_completo varchar(100),
	@correo varchar(100),
	@telefono varchar(50),
	@estado bit,

	@id_usuario_resultado int output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @id_usuario_resultado = 0
	SET @mensaje = ''
	IF(NOT EXISTS(SELECT * from Usuario WHERE documento = @documento))
	BEGIN
		INSERT INTO Usuario(id_rol, documento, clave, nombre_completo, correo, telefono, estado)
		VALUES(@id_rol, @documento, @clave, @nombre_completo, @correo, @telefono, @estado)

		SET @id_usuario_resultado = SCOPE_IDENTITY() /* guarda el ultimo id del registro ingresado*/
	END
	ELSE
		SET @mensaje = 'No se puede ingresar un usuario repetido'
END
GO

/* PROCEDIMIENTO PARA EDITAR UN USUARIO */
CREATE PROCEDURE SP_EditarUsuario(
	@id_usuario int,
	@id_rol int,
	@documento varchar(50),
	@clave varchar(50),
	@nombre_completo varchar(100),
	@correo varchar(100),
	@telefono varchar(50),
	@estado bit,

	@respuesta bit output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @respuesta = 0
	SET @mensaje = ''
	IF(NOT EXISTS(SELECT * from Usuario WHERE documento = @documento AND id_usuario != @id_usuario))
	BEGIN
		UPDATE Usuario SET
		id_rol = @id_rol,
		documento = @documento,
		clave = @clave,
		nombre_completo = @nombre_completo,
		correo = @correo,
		telefono = @telefono,
		estado = @estado

		WHERE id_usuario = @id_usuario

		SET @respuesta = 1 /* guarda el ultimo id del registro ingresado*/
	END
	ELSE
		SET @mensaje = 'No se puede ingresar un usuario repetido'
END
GO


/* PROCEDIMIENTO PARA ELIMINAR UN USUARIO */
CREATE PROCEDURE SP_EliminarUsuario(
	@id_usuario int,

	@respuesta bit output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @respuesta = 0
	SET @mensaje = ''
	declare @paso_reglas bit = 1

	IF EXISTS(SELECT * FROM Compra C
	INNER JOIN Usuario U ON C.id_usuario = U.id_usuario
	WHERE U.id_usuario = @id_usuario
	)
	BEGIN
		SET @paso_reglas = 0
		SET @respuesta = 0
		SET @mensaje = @mensaje + 'No se puede eliminar poruqe el usuario se encuantra relacionado a una COMPRA\n'
	END

	IF EXISTS(SELECT * FROM Venta V
	INNER JOIN Usuario U ON V.id_usuario = U.id_usuario
	WHERE U.id_usuario = @id_usuario
	)
	BEGIN
		SET @paso_reglas = 0
		SET @respuesta = 0
		SET @mensaje = @mensaje + 'No se puede eliminar porqUe el usuario se encuentra relacionado a una VENTA\n'
	END

	IF(@paso_reglas = 1)
	BEGIN
		DELETE Usuario WHERE id_usuario = @id_usuario
		SET @respuesta = 1
	END
END
GO

/************* TABLA CATEGORIA ************/

/* PROCEDIMINETO ALMACENADO REGISTRAR CATEGORIA */
CREATE PROC SP_RegistrarCategoria(
	@descripcion varchar(100),
	@estado bit,

	@resultado int output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 0
	IF NOT EXISTS(SELECT * FROM Categoria WHERE descripcion = @descripcion)
	BEGIN
		INSERT INTO Categoria(descripcion, estado) VALUES (@descripcion, @estado)
		SET @resultado = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		SET @mensaje = 'No se puede repetir la descripcion de una categoria'
	END
END
GO

CREATE PROC SP_EditarCategoria(
	@id_categoria int,
	@descripcion varchar(100),
	@estado bit,

	@resultado bit output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 1
	IF NOT EXISTS(SELECT * FROM Categoria WHERE descripcion = @descripcion AND id_categoria != @id_categoria)
	BEGIN
		UPDATE Categoria SET
		descripcion = @descripcion,
		estado = @estado
		WHERE id_categoria = @id_categoria
	END
	ELSE
	BEGIN
		SET @resultado = 0
		SET @mensaje = 'No se puede repetir la descripcion de una categoria'
	END
END
GO

/* PROCEDIMINETO ALMACENADO ELIMINAR CATEGORIA */
CREATE PROC SP_EliminarCategoria(
	@id_categoria int,

	@resultado bit output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 1
	IF NOT EXISTS(SELECT * FROM Categoria C 
					INNER JOIN Producto P ON C.id_categoria = P.id_categoria
					WHERE C.id_categoria = @id_categoria)
	BEGIN
		DELETE TOP(1) FROM Categoria WHERE id_categoria = @id_categoria
	END
	ELSE
	BEGIN
		SET @resultado = 0
		SET @mensaje = 'La categoría se encuentra relacionado a un producto'
	END
END
GO


SELECT id_categoria, descripcion, estado FROM Categoria

INSERT INTO Categoria(descripcion, estado) VALUES('Jeans', 1)
INSERT INTO Categoria(descripcion, estado) VALUES('Drill', 1)
INSERT INTO Categoria(descripcion, estado) VALUES('Oversize', 1)


/************* TABLA PRODUCTO ************/

/* PROCEDIMINETO ALMACENADO REGISTRAR PRODUCTOS */
CREATE PROC SP_RegistrarProducto(
	@codigo varchar(50),
	@nombre varchar(100),
	@descripcion varchar(100),
	@id_categoria int,
	@estado bit,

	@resultado int output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 0
	IF NOT EXISTS(SELECT * FROM Producto WHERE codigo = @codigo)
	BEGIN
		INSERT INTO Producto(codigo, nombre, descripcion, id_categoria, estado) VALUES (@codigo, @nombre, @descripcion, @id_categoria,  @estado)
		SET @resultado = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		SET @mensaje = 'Ya existe un producto con el mismo codigo'
	END
END
GO

	/* PROCEDIMINETO ALMACENADO EDITAR PRODUCTO */
CREATE PROC SP_EditarProducto(
	@id_producto int,
	@codigo varchar(50),
	@nombre varchar(100),
	@descripcion varchar(100),
	@id_categoria int,
	@estado bit,

	@resultado bit output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 1
	IF NOT EXISTS(SELECT * FROM Producto WHERE codigo = @codigo AND id_producto != @id_producto)
	BEGIN
		UPDATE Producto SET
		codigo = @codigo,
		nombre = @nombre,
		descripcion = @descripcion,
		id_categoria = @id_categoria,
		estado = @estado
		WHERE id_producto = @id_producto
	END
	ELSE
	BEGIN
		SET @resultado = 0
		SET @mensaje = 'Ya existe un producto con el mismo codigo'
	END
END
GO

/* PROCEDIMINETO ALMACENADO ELIMINAR CATEGORIA */
CREATE PROC SP_EliminarProducto(
	@id_producto int,

	@respuesta bit output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @respuesta = 0
	SET @mensaje = ''
	declare @paso_reglas bit = 1
	IF EXISTS(SELECT * FROM Detalle_Compra DC 
					INNER JOIN Producto P ON P.id_producto = DC.id_producto
					WHERE P.id_producto = @id_producto)
	BEGIN
		SET @paso_reglas = 0
		SET @respuesta = 0
		SET @mensaje += @mensaje + 'No se puede eliminar porque se encuentra relacionado a una COMPRA'
	END

	IF EXISTS(SELECT * FROM Detalle_Venta DV
					INNER JOIN Producto P ON P.id_producto = DV.id_producto
					WHERE P.id_producto = @id_producto)
	BEGIN
		SET @paso_reglas = 0
		SET @respuesta = 0
		SET @mensaje += @mensaje + 'No se puede eliminar porque se encuentra relacionado a una VENTA'
	END

	IF(@paso_reglas = 1)
	BEGIN
		DELETE FROM Producto WHERE id_producto = @id_producto
		SET @respuesta = 1
	END
END
GO
/*
SELECT P.id_producto, codigo, nombre, P.descripcion, P.id_categoria, C.descripcion[descripcion_categoria], P.stock, P.precio_compra, P.precio_venta, P.estado FROM Producto P
INNER JOIN Categoria C ON C.id_categoria = P.id_categoria

select * from Producto
select * from Categoria
UPDATE Producto set Estado = 1
INSERT INTO Producto(codigo, nombre, descripcion, id_categoria, estado) VALUES('101010','Gasesa','1 Litro', 6, 1)

SELECT P.id_producto, P.id_categoria, nombre, P.descripcion, C.descripcion[descripcion_categoria], P.stock, P.precio_compra, P.precio_venta, P.estado 
FROM Producto P INNER JOIN Categoria C ON C.id_categoria = P.id_categoria*/

/************* TABLA CLIENTE ************/

/* PROCEDIMINETO ALMACENADO REGISTRAR CLIENTES */
CREATE PROC SP_RegistrarCliente(
	@documento varchar(50),
	@nombre_completo varchar(100),
	@correo varchar(100),
	@telefono varchar(50),
	@estado bit,

	@resultado int output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 0
	DECLARE @id_persona INT

	IF NOT EXISTS(SELECT * FROM Cliente WHERE documento = @documento)
	BEGIN
		INSERT INTO Cliente(documento, nombre_completo, correo, telefono, estado) VALUES (@documento, @nombre_completo, @correo, @telefono,  @estado)
		SET @resultado = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		SET @mensaje = 'El cliente ya ha sido registrado'
	END
END
GO

	/* PROCEDIMINETO ALMACENADO EDITAR CLIENTE */
CREATE PROC SP_EditarCliente(
	@id_cliente int,
	@documento varchar(50),
	@nombre_completo varchar(100),
	@correo varchar(100),
	@telefono varchar(50),
	@estado bit,

	@resultado int output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 1
	IF NOT EXISTS(SELECT * FROM Cliente WHERE documento = @documento AND id_cliente != @id_cliente)
	BEGIN
		UPDATE Cliente SET
		documento = @documento,
		nombre_completo = @nombre_completo,
		correo = @correo,
		telefono = @telefono,
		estado = @estado
		WHERE id_cliente = @id_cliente
	END
	ELSE
	BEGIN
		SET @resultado = 0
		SET @mensaje = 'El cliente ya ha sido registrado'
	END
END
GO


SELECT id_cliente, documento, nombre_completo, correo, telefono, estado FROM Cliente


select * from Cliente


/************* TABLA PROVEEDORES ************/

/* PROCEDIMINETO ALMACENADO REGISTRAR CLIENTES */
CREATE PROC SP_RegistrarProveedor(
	@documento varchar(50),
	@razon_social varchar(100),
	@correo varchar(100),
	@telefono varchar(50),
	@estado bit,

	@resultado int output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 0
	DECLARE @id_persona INT

	IF NOT EXISTS(SELECT * FROM Proveedor WHERE documento = @documento)
	BEGIN
		INSERT INTO Proveedor(documento, razon_social, correo, telefono, estado) VALUES (@documento, @razon_social, @correo, @telefono,  @estado)
		SET @resultado = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		SET @mensaje = 'El cliente ya ha sido registrado'
	END
END
GO

/* PROCEDIMINETO ALMACENADO EDITAR CLIENTE */
CREATE PROC SP_EditarProveedor(
	@id_proveedor int,
	@documento varchar(50),
	@razon_social varchar(100),
	@correo varchar(100),
	@telefono varchar(50),
	@estado bit,

	@resultado int output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 1
	IF NOT EXISTS(SELECT * FROM Proveedor WHERE documento = @documento AND id_proveedor != @id_proveedor)
	BEGIN
		UPDATE Proveedor SET
		documento = @documento,
		razon_social = @razon_social,
		correo = @correo,
		telefono = @telefono,
		estado = @estado
		WHERE id_proveedor = @id_proveedor
	END
	ELSE
	BEGIN
		SET @resultado = 0
		SET @mensaje = 'El cliente ya ha sido registrado'
	END
END
GO

/* PROCEDIMINETO ALMACENADO ELIMINAR CLIENTE */
CREATE PROC SP_EliminarProveedor(
	@id_proveedor int,

	@resultado int output,
	@mensaje varchar(500) output
)
AS
BEGIN
	SET @resultado = 1
	IF NOT EXISTS(SELECT * FROM Proveedor P
	INNER JOIN Compra C ON C.id_proveedor = P.id_proveedor
	WHERE P.id_proveedor = @id_proveedor)
	BEGIN
		DELETE TOP(1) FROM Proveedor 
		WHERE id_proveedor = @id_proveedor
	END
	ELSE
	BEGIN
		SET @resultado = 0
		SET @mensaje = 'El proveedor se encuentra relacionado a una compra'
	END
END
GO

SELECT id_proveedor, documento, razon_social, correo, telefono, estado FROM Proveedor 


/************* CREACION DE TABAL DE NEGOCIO ****************/
CREATE TABLE Negocio(
	id_negocio int primary key,
	nombre varchar(100),
	ruc varchar(50),
	direccion varchar(50),
	logo varbinary(max) null
) 
go

select * from Negocio
INSERT INTO Negocio(id_negocio, nombre, ruc, direccion) VALUES (1, 'URBAN SHOP', '2015487895', 'Av. Los Angeles 12')


/* PROCESO PARA REGISTRAR UNA COMPRA */
CREATE TYPE EDetalle_Compra AS TABLE(
	id_producto int null,
	precio_compra decimal(10,2) null,
	precio_venta decimal(10,2) null,
	cantidad int null,
	monto_total decimal(10,2) null
)
GO

CREATE PROC sp_RegistrarCompra(
	@id_usuario int,
	@id_proveedor int,
	@tipo_documento varchar(500),
	@numero_documento varchar(50),
	@monto_total decimal(10,2),
	@detalle_compra EDetalle_Compra READONLY,
	@resultado bit output,
	@mensaje varchar(500) output
)
AS
BEGIN
	BEGIN TRY
		declare @id_compra int = 0
		set @resultado = 1
		set @mensaje = ''

		BEGIN TRANSACTION registro
			INSERT INTO Compra(id_usuario, id_proveedor, tipo_documento, numero_documento, monto_total) 
			VALUES(@id_usuario, @id_proveedor, @tipo_documento, @numero_documento, @monto_total)
			set @id_compra = SCOPE_IDENTITY() -- devuelve el id del ultimos registro ingresado

			INSERT INTO Detalle_Compra(id_compra, id_producto, precio_compra, precio_venta, cantidad, monto_total)
			SELECT @id_compra, id_producto, precio_compra, precio_venta, cantidad, monto_total FROM @detalle_compra -- recuprar registros de la tabla tempral

			UPDATE P SET P.stock = P.stock +  DC.cantidad,
			P.precio_compra = DC.precio_compra,
			P.precio_venta = DC.precio_venta
			FROM Producto P INNER JOIN @detalle_compra DC ON DC.id_producto = P.id_producto

		COMMIT TRANSACTION registro
	END TRY
	BEGIN CATCH
		SET @resultado = 0
		SET @mensaje = ERROR_MESSAGE()
		ROLLBACK TRANSACTION registro
	END CATCH
END
GO


select * FROM COMPRA
select * from detalle_compra
select * from Producto