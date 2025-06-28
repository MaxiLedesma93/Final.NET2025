-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 26-04-2024 a las 22:18:49
-- Versión del servidor: 10.4.24-MariaDB
-- Versión de PHP: 8.1.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `ledesmalillo`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `IdContrato` int(11) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `FecInicio` datetime NOT NULL,
  `FecFin` datetime NOT NULL,
  `Monto` decimal(10,0) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`IdContrato`, `InquilinoId`, `InmuebleId`, `FecInicio`, `FecFin`, `Monto`, `Estado`) VALUES
(12, 2, 5, '2024-04-23 14:20:00', '2024-07-31 14:20:00', '90000', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `IdInmueble` int(11) NOT NULL,
  `Direccion` varchar(100) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Superficie` int(11) NOT NULL,
  `Latitud` decimal(10,0) NOT NULL,
  `Longitud` decimal(10,0) NOT NULL,
  `Uso` varchar(100) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `TipoId` int(11) NOT NULL,
  `Importe` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`IdInmueble`, `Direccion`, `Ambientes`, `Superficie`, `Latitud`, `Longitud`, `Uso`, `PropietarioId`, `TipoId`, `Importe`) VALUES
(5, 'Colon 123', 4, 80, '12', '13', 'residencial', 2, 4, 90000),
(6, 'Maipu 2546', 5, 100, '33', '45', 'Comercial', 3, 4, 80000),
(8, 'Mendoza 123', 3, 78, '12', '13', 'Residencial', 7, 6, 80000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `IdInquilino` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Dni` varchar(10) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Email` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`IdInquilino`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`) VALUES
(2, 'Nelson', 'jofre', '28', '29', 'fabian@mail.com'),
(3, 'Gonzalo', 'jofre', '24', '25', 'samuel@mail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `IdPago` int(11) NOT NULL,
  `NumPago` int(11) NOT NULL,
  `FechaPago` datetime NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `Importe` decimal(10,0) NOT NULL,
  `Detalle` varchar(100) NOT NULL,
  `Est` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`IdPago`, `NumPago`, `FechaPago`, `ContratoId`, `Importe`, `Detalle`, `Est`) VALUES
(30, 1, '2024-04-24 00:00:00', 12, '56000', 'Abril', 1),
(31, 2, '2024-04-25 00:00:00', 12, '56000', 'Mayo', 1),
(32, 1, '2024-04-25 00:00:00', 12, '56000', 'Abril', 0),
(33, 2, '2024-04-30 00:00:00', 12, '90000', 'Efectivo', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `IdPropietario` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Dni` varchar(10) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Clave` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`IdPropietario`, `Nombre`, `Apellido`, `Dni`, `Email`, `Telefono`, `Clave`) VALUES
(2, 'Sandra', 'Lillo', '28732977', 'lula@mail.com', '2645092927', '123'),
(3, 'Fabian', 'Jofre', '28742651', 'nelson@mail.com', '1234', '123'),
(6, 'Mariano Ramiro', 'Lillo', '111111', 'mariano@mail.com', '222222', '123'),
(7, 'Jose', 'Lopez', '1', 'jose@mail.com', '2', '');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipos`
--

CREATE TABLE `tipos` (
  `IdTipo` int(11) NOT NULL,
  `Descripcion` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `tipos`
--

INSERT INTO `tipos` (`IdTipo`, `Descripcion`) VALUES
(4, 'Local'),
(5, 'Depósito'),
(6, 'Casa'),
(7, 'Departamento'),
(8, 'Quinta');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `IdUsuario` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `Clave` varchar(100) NOT NULL,
  `Avatar` varchar(200) NOT NULL,
  `Rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`IdUsuario`, `Nombre`, `Apellido`, `Email`, `Clave`, `Avatar`, `Rol`) VALUES
(54, 'Maximiliano Alberto', 'Ledesma', 'maximiliano@gmail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', ' ', 1),
(55, 'Lucila', 'Lillo', 'lula@mail.com', '3A0G2+zJ3luLnlC44+Xe5HGw/9RWJNoyF2XZACvev20=', '', 1),
(56, 'Marcos', 'Galperin', 'marcos@gmail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', '', 2);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`IdContrato`),
  ADD KEY `contratos_ibfk_1` (`InmuebleId`),
  ADD KEY `contratos_ibfk_2` (`InquilinoId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`IdInmueble`),
  ADD KEY `PropietarioId` (`PropietarioId`),
  ADD KEY `TipoId` (`TipoId`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`IdInquilino`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`IdPago`),
  ADD KEY `ContratoId` (`ContratoId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`IdPropietario`);

--
-- Indices de la tabla `tipos`
--
ALTER TABLE `tipos`
  ADD PRIMARY KEY (`IdTipo`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`IdUsuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `IdInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `IdPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=34;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `IdPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `tipos`
--
ALTER TABLE `tipos`
  MODIFY `IdTipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `IdUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=57;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`IdInmueble`) ON UPDATE CASCADE,
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`IdInquilino`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`IdPropietario`),
  ADD CONSTRAINT `inmuebles_ibfk_2` FOREIGN KEY (`TipoId`) REFERENCES `tipos` (`IdTipo`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`IdContrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
