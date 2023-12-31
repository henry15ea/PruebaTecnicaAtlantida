USE [master]
GO
/****** Object:  Database [AtlantidaDB]    Script Date: 11/4/2023 8:22:12 PM ******/
CREATE DATABASE [AtlantidaDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AtlantidaDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AtlantidaDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AtlantidaDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AtlantidaDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [AtlantidaDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AtlantidaDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AtlantidaDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AtlantidaDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AtlantidaDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AtlantidaDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AtlantidaDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [AtlantidaDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [AtlantidaDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AtlantidaDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AtlantidaDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AtlantidaDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AtlantidaDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AtlantidaDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AtlantidaDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AtlantidaDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AtlantidaDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [AtlantidaDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AtlantidaDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AtlantidaDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AtlantidaDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AtlantidaDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AtlantidaDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AtlantidaDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AtlantidaDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AtlantidaDB] SET  MULTI_USER 
GO
ALTER DATABASE [AtlantidaDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AtlantidaDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AtlantidaDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AtlantidaDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AtlantidaDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AtlantidaDB] SET QUERY_STORE = OFF
GO
USE [AtlantidaDB]
GO
/****** Object:  UserDefinedFunction [dbo].[calcularCuotaMinimaPagar]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[calcularCuotaMinimaPagar](@saldoTotal decimal(10,2), @porcentajeConfigurableSaldoMinimo decimal(5,2))
RETURNS decimal(10,2)
AS
BEGIN
    DECLARE @cuotaMinimaPagar decimal(10,2)
    SET @cuotaMinimaPagar = @saldoTotal * @porcentajeConfigurableSaldoMinimo
    RETURN @cuotaMinimaPagar
END
GO
/****** Object:  UserDefinedFunction [dbo].[calcularInteresBonificable]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[calcularInteresBonificable](@saldoTotal decimal(10,2), @porcentajeInteresConfigurable decimal(5,2))
RETURNS decimal(10,2)
AS
BEGIN
    DECLARE @interesBonificable decimal(10,2)
    SET @interesBonificable = @saldoTotal * @porcentajeInteresConfigurable
    RETURN @interesBonificable
END
GO
/****** Object:  UserDefinedFunction [dbo].[calcularMontoTotalContadoInteresPagar]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[calcularMontoTotalContadoInteresPagar](@saldoTotal decimal(10,2), @porcentajeInteresConfigurable decimal(5,2))
RETURNS decimal(10,2)
AS
BEGIN
    DECLARE @montoTotalContadoInteres decimal(10,2)
    SET @montoTotalContadoInteres = @saldoTotal + (dbo.calcularInteresBonificable(@saldoTotal, @porcentajeInteresConfigurable))
    RETURN @montoTotalContadoInteres
END
GO
/****** Object:  UserDefinedFunction [dbo].[calcularMontoTotalContadoPagar]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[calcularMontoTotalContadoPagar](@saldoTotal decimal(10,2))
RETURNS decimal(10,2)
AS
BEGIN
    DECLARE @montoTotalContado decimal(10,2)
    SET @montoTotalContado = @saldoTotal
    RETURN @montoTotalContado
END
GO
/****** Object:  Table [dbo].[tbl_compras]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_compras](
	[id_compra] [varchar](64) NOT NULL,
	[fechaCompra] [datetime2](7) NULL,
	[descripcionCompta] [varchar](120) NOT NULL,
	[montoCompra] [decimal](10, 2) NOT NULL,
	[id_cuenta] [varchar](64) NOT NULL,
	[registro] [datetime2](7) NULL,
 CONSTRAINT [pk_compra] PRIMARY KEY CLUSTERED 
(
	[id_compra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_departament]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_departament](
	[id_dep] [varchar](64) NOT NULL,
	[nmb_dep] [varchar](120) NOT NULL,
 CONSTRAINT [pk_dep] PRIMARY KEY CLUSTERED 
(
	[id_dep] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_EstadoCompra]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_EstadoCompra](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](10) NULL,
	[registro] [datetime2](7) NULL,
 CONSTRAINT [pk_ecompra] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_municipio]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_municipio](
	[id_mun] [varchar](64) NOT NULL,
	[id_dep] [varchar](64) NOT NULL,
	[nmb_mun] [varchar](120) NOT NULL,
 CONSTRAINT [pk_mun] PRIMARY KEY CLUSTERED 
(
	[id_mun] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_pagos]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_pagos](
	[id_pago] [varchar](64) NOT NULL,
	[fechaPago] [date] NOT NULL,
	[montoPago] [decimal](10, 2) NOT NULL,
	[id_cuenta] [varchar](64) NOT NULL,
	[registroPago] [datetime2](7) NULL,
 CONSTRAINT [pk_pagoid] PRIMARY KEY CLUSTERED 
(
	[id_pago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_rolesCuenta]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_rolesCuenta](
	[id_rol] [varchar](64) NOT NULL,
	[nombre] [varchar](25) NOT NULL,
 CONSTRAINT [pk_rl] PRIMARY KEY CLUSTERED 
(
	[id_rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TokenType]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TokenType](
	[id] [int] NOT NULL,
	[typeName] [varchar](10) NOT NULL,
 CONSTRAINT [pk_tkntype] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_usuarioCredenciales]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_usuarioCredenciales](
	[id_credencial] [varchar](64) NOT NULL,
	[uname] [varchar](25) NOT NULL,
	[pass] [varchar](64) NOT NULL,
	[tipoCuenta] [varchar](64) NOT NULL,
	[estado] [char](1) NOT NULL,
	[fecha_registro] [datetime2](7) NULL,
 CONSTRAINT [pk_credentialU] PRIMARY KEY CLUSTERED 
(
	[id_credencial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_usuarioCuentas]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_usuarioCuentas](
	[id] [varchar](64) NOT NULL,
	[id_uInfo] [varchar](64) NOT NULL,
	[numeroTargeta] [varchar](19) NOT NULL,
	[deudaActual] [decimal](10, 2) NOT NULL,
	[limiteCredito] [decimal](10, 2) NOT NULL,
	[saldoDisponible] [decimal](10, 2) NOT NULL,
	[estado] [char](1) NOT NULL,
	[fecha_registro] [datetime2](7) NULL,
	[fechaUltimoPago] [datetime2](7) NULL,
	[interesBonificable] [decimal](10, 2) NOT NULL,
	[porcentajeInteresConfigurable] [decimal](5, 2) NOT NULL,
	[montoTotalComprasMesActual] [decimal](10, 2) NOT NULL,
	[montoTotalComprasMesAnterior] [decimal](10, 2) NOT NULL,
	[porcentajeConfigurableSaldoMinimo] [decimal](5, 2) NOT NULL,
	[cuotaMinimaPagar] [decimal](10, 2) NOT NULL,
	[montoTotalContado] [decimal](10, 2) NOT NULL,
	[montoTotalContadoInteres] [decimal](10, 2) NOT NULL,
	[saldoTotal] [decimal](10, 2) NOT NULL,
 CONSTRAINT [pk_ucuentas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_usuarioInfo]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_usuarioInfo](
	[id_uInfo] [varchar](64) NOT NULL,
	[nombre] [varchar](120) NOT NULL,
	[apellidos] [varchar](220) NOT NULL,
	[edad] [tinyint] NOT NULL,
	[correo] [varchar](120) NOT NULL,
	[ndoc] [varchar](11) NOT NULL,
	[direccion] [varchar](120) NOT NULL,
	[id_mun] [varchar](64) NOT NULL,
	[estado] [char](1) NOT NULL,
	[fecha_registro] [datetime2](7) NULL,
 CONSTRAINT [pk_uinfo] PRIMARY KEY CLUSTERED 
(
	[id_uInfo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tokens]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tokens](
	[Token] [varchar](512) NOT NULL,
	[UserId] [varchar](64) NOT NULL,
	[ExpirationTime] [datetimeoffset](7) NOT NULL,
	[TokenType] [int] NOT NULL,
	[IssuedTime] [datetimeoffset](7) NOT NULL,
	[Revoked] [bit] NOT NULL,
 CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_compras] ADD  DEFAULT (getdate()) FOR [fechaCompra]
GO
ALTER TABLE [dbo].[tbl_compras] ADD  DEFAULT ('Compra del dia') FOR [descripcionCompta]
GO
ALTER TABLE [dbo].[tbl_compras] ADD  DEFAULT ((0.00)) FOR [montoCompra]
GO
ALTER TABLE [dbo].[tbl_compras] ADD  DEFAULT (getdate()) FOR [registro]
GO
ALTER TABLE [dbo].[tbl_departament] ADD  DEFAULT ('San Salvador') FOR [nmb_dep]
GO
ALTER TABLE [dbo].[tbl_EstadoCompra] ADD  DEFAULT ('PENDIENTE') FOR [nombre]
GO
ALTER TABLE [dbo].[tbl_EstadoCompra] ADD  DEFAULT (getdate()) FOR [registro]
GO
ALTER TABLE [dbo].[tbl_municipio] ADD  DEFAULT ('San salvador') FOR [nmb_mun]
GO
ALTER TABLE [dbo].[tbl_pagos] ADD  DEFAULT (getdate()) FOR [registroPago]
GO
ALTER TABLE [dbo].[tbl_rolesCuenta] ADD  DEFAULT ('USER') FOR [nombre]
GO
ALTER TABLE [dbo].[tbl_TokenType] ADD  DEFAULT ('Access') FOR [typeName]
GO
ALTER TABLE [dbo].[tbl_usuarioCredenciales] ADD  DEFAULT ('usuario123') FOR [uname]
GO
ALTER TABLE [dbo].[tbl_usuarioCredenciales] ADD  DEFAULT ('184400D322523F28FDDBF05195FC873930F435602F678741855BEFD2A2887188') FOR [pass]
GO
ALTER TABLE [dbo].[tbl_usuarioCredenciales] ADD  DEFAULT ('92B7B421992EF490F3B75898EC0E511F1A5C02422819D89719B20362B023EE4F') FOR [tipoCuenta]
GO
ALTER TABLE [dbo].[tbl_usuarioCredenciales] ADD  DEFAULT ('0') FOR [estado]
GO
ALTER TABLE [dbo].[tbl_usuarioCredenciales] ADD  DEFAULT (getdate()) FOR [fecha_registro]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ('1111-1111-1111-1111') FOR [numeroTargeta]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [deudaActual]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [limiteCredito]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [saldoDisponible]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ('0') FOR [estado]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT (getdate()) FOR [fecha_registro]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT (getdate()) FOR [fechaUltimoPago]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [interesBonificable]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [porcentajeInteresConfigurable]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [montoTotalComprasMesActual]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [montoTotalComprasMesAnterior]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [porcentajeConfigurableSaldoMinimo]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [cuotaMinimaPagar]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [montoTotalContado]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [montoTotalContadoInteres]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] ADD  DEFAULT ((0.00)) FOR [saldoTotal]
GO
ALTER TABLE [dbo].[tbl_usuarioInfo] ADD  DEFAULT ('Nombre') FOR [nombre]
GO
ALTER TABLE [dbo].[tbl_usuarioInfo] ADD  DEFAULT ('Apellidos') FOR [apellidos]
GO
ALTER TABLE [dbo].[tbl_usuarioInfo] ADD  DEFAULT ((18)) FOR [edad]
GO
ALTER TABLE [dbo].[tbl_usuarioInfo] ADD  DEFAULT ('usuario@mail.com') FOR [correo]
GO
ALTER TABLE [dbo].[tbl_usuarioInfo] ADD  DEFAULT ('01013121-7') FOR [ndoc]
GO
ALTER TABLE [dbo].[tbl_usuarioInfo] ADD  DEFAULT ('domicilio') FOR [direccion]
GO
ALTER TABLE [dbo].[tbl_usuarioInfo] ADD  DEFAULT ('0') FOR [estado]
GO
ALTER TABLE [dbo].[tbl_usuarioInfo] ADD  DEFAULT (getdate()) FOR [fecha_registro]
GO
ALTER TABLE [dbo].[Tokens] ADD  DEFAULT ((0)) FOR [Revoked]
GO
ALTER TABLE [dbo].[tbl_compras]  WITH CHECK ADD  CONSTRAINT [fk_uscomprra] FOREIGN KEY([id_cuenta])
REFERENCES [dbo].[tbl_usuarioCuentas] ([id])
GO
ALTER TABLE [dbo].[tbl_compras] CHECK CONSTRAINT [fk_uscomprra]
GO
ALTER TABLE [dbo].[tbl_municipio]  WITH CHECK ADD  CONSTRAINT [fk_deptomun] FOREIGN KEY([id_dep])
REFERENCES [dbo].[tbl_departament] ([id_dep])
GO
ALTER TABLE [dbo].[tbl_municipio] CHECK CONSTRAINT [fk_deptomun]
GO
ALTER TABLE [dbo].[tbl_pagos]  WITH CHECK ADD  CONSTRAINT [fk_uspago] FOREIGN KEY([id_cuenta])
REFERENCES [dbo].[tbl_usuarioCuentas] ([id])
GO
ALTER TABLE [dbo].[tbl_pagos] CHECK CONSTRAINT [fk_uspago]
GO
ALTER TABLE [dbo].[tbl_usuarioCredenciales]  WITH CHECK ADD  CONSTRAINT [fk_cuentaUsuario] FOREIGN KEY([id_credencial])
REFERENCES [dbo].[tbl_usuarioCuentas] ([id])
GO
ALTER TABLE [dbo].[tbl_usuarioCredenciales] CHECK CONSTRAINT [fk_cuentaUsuario]
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas]  WITH CHECK ADD  CONSTRAINT [fk_uinfo] FOREIGN KEY([id_uInfo])
REFERENCES [dbo].[tbl_usuarioInfo] ([id_uInfo])
GO
ALTER TABLE [dbo].[tbl_usuarioCuentas] CHECK CONSTRAINT [fk_uinfo]
GO
ALTER TABLE [dbo].[tbl_usuarioInfo]  WITH CHECK ADD  CONSTRAINT [fk_muni] FOREIGN KEY([id_mun])
REFERENCES [dbo].[tbl_municipio] ([id_mun])
GO
ALTER TABLE [dbo].[tbl_usuarioInfo] CHECK CONSTRAINT [fk_muni]
GO
ALTER TABLE [dbo].[Tokens]  WITH CHECK ADD  CONSTRAINT [fk_tokenType] FOREIGN KEY([TokenType])
REFERENCES [dbo].[tbl_TokenType] ([id])
GO
ALTER TABLE [dbo].[Tokens] CHECK CONSTRAINT [fk_tokenType]
GO
ALTER TABLE [dbo].[Tokens]  WITH CHECK ADD  CONSTRAINT [fk_user] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbl_usuarioCredenciales] ([id_credencial])
GO
ALTER TABLE [dbo].[Tokens] CHECK CONSTRAINT [fk_user]
GO
/****** Object:  StoredProcedure [dbo].[InsertarPagoCompleto]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertarPagoCompleto]
@id_pago varchar(64) ,
@fechaPago date,
@montoPago decimal(10,2),
@id_cuenta varchar(64),
@return_value INT OUTPUT
AS
BEGIN
SET NOCOUNT ON

    -- Verificar que el monto de pago no exceda la deuda actual
    DECLARE @deudaActual DECIMAL(10,2)
    SELECT @deudaActual = deudaActual
    FROM AtlantidaDB.dbo.tbl_usuarioCuentas
    WHERE id = @id_cuenta

    IF @montoPago > @deudaActual
    BEGIN
        -- Retornar 0 si el monto de pago excede la deuda actual
        SET @return_value = 0
    END
    ELSE
    BEGIN
        -- Insertar el pago en la tabla tbl_pagos
        INSERT INTO tbl_pagos (id_pago, fechaPago, montoPago, id_cuenta)
        VALUES (@id_pago, @fechaPago, @montoPago, @id_cuenta)

        -- Actualizar los campos saldo_disponible y deuda_actual
        UPDATE AtlantidaDB.dbo.tbl_usuarioCuentas
        SET saldoDisponible = saldoDisponible + @montoPago,
            deudaActual = deudaActual - @montoPago
        WHERE id = @id_cuenta

        -- Retornar 1 si todo se ejecutó correctamente
        SET @return_value = 1
    END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateCompras]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateCompras]
@id_compra varchar(64),
@fechaCompra datetime2 ,
@descripcionCompta varchar(120),
@montoCompra decimal(10,2),
@id_cuenta varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY
INSERT INTO tbl_compras (id_compra,fechaCompra,descripcionCompta,montoCompra,id_cuenta)
VALUES 
(
@id_compra,
@fechaCompra,
@descripcionCompta,
@montoCompra,
@id_cuenta
)
SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateComprasComplete]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateComprasComplete]
@id_compra varchar(64),
@fechaCompra datetime2 ,
@descripcionCompta varchar(120),
@montoCompra decimal(10,2),
@id_cuenta varchar(64),
@limiteCreditoUsuario decimal(10,2),
@return_value int OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    DECLARE @result int
    SET @result = 0
    
    BEGIN TRY
        DECLARE @saldoDisponible decimal(10,2)
        SELECT @saldoDisponible = saldoDisponible
        FROM tbl_usuarioCuentas
        WHERE id = @id_cuenta
        
        IF (@saldoDisponible >= @montoCompra) -- Hay suficiente saldo disponible
        BEGIN
            INSERT INTO tbl_compras (id_compra, fechaCompra, descripcionCompta, montoCompra, id_cuenta)
            VALUES (@id_compra, @fechaCompra, @descripcionCompta, @montoCompra, @id_cuenta)
            
            SET @result = 1
            
            -- Actualizar saldoDisponible y deudaActual en tbl_usuarioCuentas
            UPDATE tbl_usuarioCuentas
            SET saldoDisponible = saldoDisponible - @montoCompra,
                deudaActual = deudaActual + @montoCompra
            WHERE id = @id_cuenta
        END
        ELSE -- No hay suficiente saldo disponible, utilizar el límite de crédito
        BEGIN
            DECLARE @saldoTotal decimal(10,2)
            SELECT @saldoTotal = saldoDisponible + @limiteCreditoUsuario
            FROM tbl_usuarioCuentas
            WHERE id = @id_cuenta
            
            IF (@saldoTotal >= @montoCompra) -- El límite de crédito es suficiente
            BEGIN
                INSERT INTO tbl_compras (id_compra, fechaCompra, descripcionCompta, montoCompra, id_cuenta)
                VALUES (@id_compra, @fechaCompra, @descripcionCompta, @montoCompra, @id_cuenta)
                
                SET @result = 1
                
                -- Actualizar saldoDisponible, deudaActual y limiteCredito en tbl_usuarioCuentas
                UPDATE tbl_usuarioCuentas
                SET saldoDisponible = 0,
                    deudaActual = deudaActual + @montoCompra,
                    limiteCredito = @limiteCreditoUsuario - (@montoCompra - @saldoDisponible)
                WHERE id = @id_cuenta
            END
            ELSE -- El límite de crédito no es suficiente
            BEGIN
                SET @result = 0
            END
        END
    END TRY
    BEGIN CATCH
        SET @result = 0
    END CATCH
    
    SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreatePagos]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreatePagos]
@id_pago varchar(64) ,
@fechaPago date,
@montoPago decimal(10,2),
@id_cuenta varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY
INSERT INTO tbl_pagos (id_pago,fechaPago,montoPago,id_cuenta)
VALUES 
(
@id_pago,
@fechaPago,
@montoPago,
@id_cuenta
)
SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateUsuarioCredenciales]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateUsuarioCredenciales]
@id_credencial varchar(64) ,
@uname varchar(25) ,
@pass varchar(64) ,
@tipoCuenta varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY
INSERT INTO tbl_usuarioCredenciales (id_credencial,uname,pass,tipoCuenta)
VALUES 
(
@id_credencial,
@uname,
@pass,
@tipoCuenta
)
SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateUsuarioCuentas]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateUsuarioCuentas]
@id varchar(64),
@id_uInfo varchar(64),
@numeroTargeta varchar(19) ,
@deudaActual decimal(10,2) ,
@limiteCredito decimal(10,2),
@saldoDisponible decimal(10,2),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY
INSERT INTO tbl_usuarioCuentas (id,id_uInfo,numeroTargeta,deudaActual,limiteCredito,saldoDisponible) 
VALUES
(
@id,
@id_uInfo,
@numeroTargeta,
@deudaActual,
@limiteCredito,
@saldoDisponible
)

SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_createUsuarioInfo]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_createUsuarioInfo]
@id_uInfo varchar(64),
@nombre varchar(120),
@apellidos varchar(220),
@edad tinyint  ,
@correo varchar(120) ,
@ndoc varchar(11)  ,
@direccion varchar(120),
@id_mun varchar(64),
@estado char(1),
@return_value int OUTPUT
as
begin
	SET NOCOUNT ON
	DECLARE @result int
	SET @result = 0
	BEGIN TRY
	INSERT INTO tbl_usuarioInfo (id_uInfo,nombre,apellidos,edad,correo,ndoc,direccion,id_mun,estado) VALUES 
	(@id_uInfo,@nombre,@apellidos,@edad,@correo,@ndoc,@direccion,@id_mun,@estado)
SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
end
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteCompras]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteCompras]
@id_compra varchar(64),
@fechaCompra datetime2 ,
@descripcionCompta varchar(120),
@montoCompra decimal(10,2),
@id_cuenta varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY
DELETE FROM tbl_compras WHERE  id_compra=@id_compra AND id_cuenta=@id_cuenta
SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeletePago]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_DeletePago]
@id_pago varchar(64) ,
@id_cuenta varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY
DELETE FROM tbl_pagos WHERE  id_pago=@id_pago AND id_cuenta=@id_cuenta
SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteUsuarioCredenciales]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteUsuarioCredenciales]
@id_credencial varchar(64) ,
@uname varchar(25) ,
@pass varchar(64) ,
@tipoCuenta varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY
DELETE FROM tbl_usuarioCredenciales WHERE  id_credencial=@id_credencial
SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteUsuarioCuentas]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteUsuarioCuentas]
@id_uInfo varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY
DELETE FROM tbl_usuarioCuentas WHERE id_uInfo = @id_uInfo

SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_deleteUsuarioInfo]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_deleteUsuarioInfo]
@id_uInfo varchar(64),
@correo varchar(120) ,
@return_value int OUTPUT
as
begin
	SET NOCOUNT ON
	DECLARE @result int
	SET @result = 0
	BEGIN TRY
	DELETE FROM tbl_usuarioInfo WHERE  id_uInfo=@id_uInfo AND correo=@correo
SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
end
GO
/****** Object:  StoredProcedure [dbo].[sp_loginUsuarioCredenciales]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_loginUsuarioCredenciales]
@uname varchar(25) ,
@pass varchar(64) 
AS
BEGIN
    SELECT 
ucr.id_credencial 
	FROM tbl_usuarioCredenciales ucr 
	inner join tbl_rolesCuenta trc on ucr.tipoCuenta = trc.id_rol  
    WHERE 
        ucr.uname = @uname
        AND ucr.pass = @pass
END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectCompras]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectCompras]
@id_cuenta varchar(64)
AS
BEGIN
    select tc.id_compra ,tc.descripcionCompta,tc.montoCompra,tc.fechaCompra  
    from tbl_compras tc  where tc.id_cuenta = @id_cuenta

END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectComprasByFecha]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectComprasByFecha]
@id_cuenta varchar(64),
@fechaCompra datetime2
AS
BEGIN
    select tc.id_compra ,tc.descripcionCompta,tc.montoCompra,tc.fechaCompra  
    from tbl_compras tc  where tc.id_cuenta = @id_cuenta AND tc.fechaCompra  =@fechaCompra

END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectComprasByFechaMes]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectComprasByFechaMes]
@id_cuenta varchar(64),
@fechaInicio datetime2,
@fechaFinal datetime2
AS
BEGIN
    SELECT 
		    id_compra,
		    descripcionCompta,
		    montoCompra,
		    fechaCompra
		FROM tbl_compras 
		WHERE 
		    id_cuenta = @id_cuenta AND 
		    fechaCompra BETWEEN @fechaInicio AND @fechaFinal

END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectLastPagos]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectLastPagos]
@id_cuenta varchar(64)
AS
BEGIN
      SELECT 
   TOP 5
tp.id_pago ,
tp.montoPago ,
tp.fechaPago
from tbl_pagos tp inner join tbl_usuarioCuentas tu on tp.id_cuenta = tu.id  
where tp.id_cuenta = @id_cuenta order by tp.fechaPago DESC

END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectPagosAll]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectPagosAll]
@id_cuenta varchar(64)
AS
BEGIN
    select 
tp.id_pago ,
tp.montoPago ,
tp.fechaPago
from tbl_pagos tp inner join tbl_usuarioCuentas tu on tp.id_cuenta = tu.id  where tp.id_cuenta = @id_cuenta

END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectPagosByFecha]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectPagosByFecha]
@id_cuenta varchar(64),
@fechaPago date
AS
BEGIN
    select 
tp.id_pago ,
tp.montoPago ,
tp.fechaPago
from tbl_pagos tp inner join tbl_usuarioCuentas tu on tp.id_cuenta = tu.id  
where tp.id_cuenta = @id_cuenta AND  tp.fechaPago =@fechaPago

END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectUsuarioCredenciales]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectUsuarioCredenciales]
@uname varchar(25) ,
@pass varchar(64) 
AS
BEGIN
    SELECT 
	ucr.id_credencial ,
	ucr.uname ,
	ucr.pass ,
	trc.nombre 
	FROM tbl_usuarioCredenciales ucr 
	inner join tbl_rolesCuenta trc on ucr.tipoCuenta = trc.id_rol 
    WHERE 
        ucr.uname = @uname
        AND ucr.pass = @pass
END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectUsuarioCredito]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectUsuarioCredito]
    @id VARCHAR(64)
AS
BEGIN
    SELECT 
        uc.deudaActual,
        uc.limiteCredito,
        uc.saldoDisponible
    FROM 
        tbl_usuarioCuentas uc
    INNER JOIN 
        tbl_usuarioInfo ui ON uc.id_uInfo = ui.id_uInfo
        where uc.id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectUsuarioCuentas]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectUsuarioCuentas]
    @id VARCHAR(64)
AS
BEGIN
    SELECT 
		uc.id ,
        ui.id_uInfo,
        ui.nombre,
        ui.apellidos,
        ui.edad,
        ui.correo,
        ui.ndoc,
        ui.direccion,
        uc.numeroTargeta,
        uc.deudaActual,
        uc.limiteCredito,
        uc.saldoDisponible
    FROM 
        tbl_usuarioCuentas uc
    INNER JOIN 
        tbl_usuarioInfo ui ON uc.id_uInfo = ui.id_uInfo
        where uc.id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectUsuarioCuentasComplete]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectUsuarioCuentasComplete]
    @id VARCHAR(64)
AS
BEGIN
    SELECT 
        uc.id,
        ui.id_uInfo,
        ui.nombre,
        ui.apellidos,
        ui.edad,
        ui.correo,
        ui.ndoc,
        ui.direccion,
        uc.numeroTargeta,
        uc.deudaActual,
        uc.limiteCredito,
        uc.saldoDisponible,
        uc.fechaUltimoPago,
        dbo.calcularInteresBonificable(uc.saldoTotal, uc.porcentajeInteresConfigurable) as interesBonificable,
        uc.porcentajeInteresConfigurable,
        uc.montoTotalComprasMesActual,
        uc.montoTotalComprasMesAnterior,
        uc.porcentajeConfigurableSaldoMinimo,
        dbo.calcularCuotaMinimaPagar(uc.saldoTotal, uc.porcentajeConfigurableSaldoMinimo) as cuotaMinimaPagar,
        dbo.calcularMontoTotalContadoPagar(uc.saldoTotal) as montoTotalContado,
        dbo.calcularMontoTotalContadoInteresPagar(uc.saldoTotal, uc.porcentajeInteresConfigurable) as montoTotalContadoInteres
    FROM 
        tbl_usuarioCuentas uc
    INNER JOIN 
        tbl_usuarioInfo ui ON uc.id_uInfo = ui.id_uInfo
    WHERE 
        uc.id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_selectUsuarioInfo]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_selectUsuarioInfo]
    @id VARCHAR(64)
AS
BEGIN
   SELECT 
tui.nombre,
          tui.apellidos,
          tui.edad,
          tui.correo,
           tui.ndoc,
           tui.direccion,
          mn.nmb_mun,
          td.nmb_dep 
    FROM tbl_usuarioCuentas tuc 
    join tbl_usuarioInfo tui  on tuc.id_uInfo = tui.id_uInfo 
    LEFT JOIN tbl_municipio mn ON tui.id_mun = mn.id_mun
    join tbl_departament td on mn.id_dep = td.id_dep 
    where tuc.id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateCompras]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateCompras]
@id_compra varchar(64),
@fechaCompra datetime2 ,
@descripcionCompta varchar(120),
@montoCompra decimal(10,2),
@id_cuenta varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY

UPDATE tbl_compras SET fechaCompra=@fechaCompra,descripcionCompta=@descripcionCompta,montoCompra=@montoCompra WHERE id_compra=@id_compra AND id_cuenta=@id_cuenta

SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdatePago]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdatePago]
@id_pago varchar(64) ,
@fechaPago date,
@montoPago decimal(10,2),
@id_cuenta varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY

INSERT INTO tbl_pagos (id_pago,fechaPago,montoPago,id_cuenta)
VALUES 
(
@id_pago,
@fechaPago,
@montoPago,
@id_cuenta
)

UPDATE tbl_pagos SET fechaPago=@fechaPago,montoPago=@montoPago WHERE id_cuenta=@id_cuenta AND id_pago=@id_pago

SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUsuarioCredenciales]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateUsuarioCredenciales]
@id_credencial varchar(64) ,
@uname varchar(25) ,
@pass varchar(64) ,
@tipoCuenta varchar(64),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY

UPDATE tbl_usuarioCredenciales SET uname=@uname,pass=@pass, tipoCuenta=@tipoCuenta WHERE id_credencial=@id_credencial

SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUsuarioCuentas]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateUsuarioCuentas]
@id varchar(64),
@id_uInfo varchar(64),
@numeroTargeta varchar(19) ,
@deudaActual decimal(10,2) ,
@limiteCredito decimal(10,2),
@saldoDisponible decimal(10,2),
@return_value int OUTPUT
AS
BEGIN
SET NOCOUNT ON
DECLARE @result int
SET @result = 0
BEGIN TRY
UPDATE tbl_usuarioCuentas SET 
id_uInfo=@id_uInfo,
numeroTargeta= @numeroTargeta,
deudaActual=@deudaActual,
limiteCredito=@limiteCredito,
saldoDisponible=@saldoDisponible
WHERE id_uInfo = @id_uInfo

SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_updateUsuarioInfo]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_updateUsuarioInfo]
@id varchar(64),
@nombre varchar(120),
@apellidos varchar(220),
@edad tinyint  ,
@correo varchar(120) ,
@ndoc varchar(11)  ,
@direccion varchar(120),
@id_mun varchar(64),
@estado char(1),
@return_value int OUTPUT
as
begin
	SET NOCOUNT ON
	DECLARE @result int
	SET @result = 0
	BEGIN TRY
	
	UPDATE tui
    SET tui.nombre = @nombre,
        tui.apellidos = @apellidos,
        tui.edad = @edad,
        tui.correo = @correo,
        tui.ndoc = @ndoc,
        tui.direccion = @direccion,
        tui.id_mun = @id_mun,
        tui.estado = @estado
    FROM tbl_usuarioInfo tui
    JOIN tbl_usuarioCuentas tuc ON tuc.id_uInfo = tui.id_uInfo
    WHERE tuc.id = @id

SET @result = 1
END TRY
BEGIN CATCH
SET @result = 0
END CATCH
SET @return_value = @result
end
GO
/****** Object:  StoredProcedure [dbo].[spTokens_CRUD]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spTokens_CRUD]
    @Token varchar(512),
    @UserId varchar(64),
    @ExpirationTime datetimeoffset(7),
    @TokenType int,
    @IssuedTime datetimeoffset(7),
    @Revoked bit,
    @Action nvarchar(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @Action = 'CREATE'
    BEGIN
        INSERT INTO [dbo].[Tokens] (
            [Token], [UserId], [ExpirationTime], [TokenType], [IssuedTime], [Revoked]
        ) VALUES (
            @Token, @UserId, @ExpirationTime, @TokenType, @IssuedTime, @Revoked
        );
    END
    ELSE IF @Action = 'READ'
    BEGIN
        SELECT [Token], [UserId], [ExpirationTime], [TokenType], [IssuedTime], [Revoked]
        FROM [dbo].[Tokens]
        WHERE [Token] = @Token;
    END
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE [dbo].[Tokens]
        SET [UserId] = @UserId, [ExpirationTime] = @ExpirationTime, [TokenType] = @TokenType,
            [IssuedTime] = @IssuedTime, [Revoked] = @Revoked
        WHERE [Token] = @Token;
    END
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM [dbo].[Tokens]
        WHERE [Token] = @Token;
    END
END;
GO
/****** Object:  StoredProcedure [dbo].[spTokens_Select]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spTokens_Select]
@Token varchar(512)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [Token], [UserId], [ExpirationTime], [TokenType], [IssuedTime], [Revoked]
        FROM [dbo].[Tokens]
        WHERE [Token] = @Token;
END
GO
/****** Object:  StoredProcedure [dbo].[spTokens_Update]    Script Date: 11/4/2023 8:22:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spTokens_Update]
    @Token varchar(512),
    @Revoked bit,
    @ReturnValue INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE [dbo].[Tokens]
        SET [Revoked] = @Revoked
        WHERE [Token] = @Token;

        SET @ReturnValue = 0; -- Actualización exitosa
    END TRY
    BEGIN CATCH
        SET @ReturnValue = 1; -- Error durante la actualización
    END CATCH;
END;

GO
USE [master]
GO
ALTER DATABASE [AtlantidaDB] SET  READ_WRITE 
GO
