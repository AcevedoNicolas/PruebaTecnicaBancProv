-- Crear la base de datos "BancoProv" 
CREATE DATABASE BancoProv;
USE BancoProv;
GO

-- Crear la tabla "TiposDocumento"
CREATE TABLE TiposDocumento (
    Id INT PRIMARY KEY IDENTITY,
    Descripcion NVARCHAR(50) NOT NULL
);
-- Crear descripcion de los tipos de documento
INSERT INTO TiposDocumento (Descripcion) VALUES ('DNI'), ('PASAPORTE');

-- Crear la tabla "Empleados"
CREATE TABLE Empleados (
    Id INT PRIMARY KEY IDENTITY,
    Codigo NCHAR(5) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    FechaNacimiento DATE,
    IdTipoDto INT NOT NULL,
    NumDocumento INT NOT NULL,
    FOREIGN KEY (IdTipoDto) REFERENCES TiposDocumento (Id)
);
