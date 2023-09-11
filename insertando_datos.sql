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


select * from usuario