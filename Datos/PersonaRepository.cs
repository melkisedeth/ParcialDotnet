using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Entidad;

namespace Datos
{
    public class PersonaRepository
    {
        private readonly SqlConnection _connection;
        private readonly List<Persona> _personas = new List<Persona>();
        public PersonaRepository(ConnectionManager connection)
        {
            _connection = connection._conexion;
        }
        public void Guardar(Persona persona)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"Insert Into personas (Identificacion,Nombre,Sexo, Edad, Departamento,Ciudad,Modalidad,
                                        Aporte,Fecha, TotalAporteEconomico) 
                                        values (@Identificacion,@Nombre,@Sexo,@Edad, @Departamento,@Ciudad,@Modalidad,
                                        @Aporte,@Fecha,@TotalAporteEconomico)";
                command.Parameters.AddWithValue("@Identificacion", persona.Identificacion);
                command.Parameters.AddWithValue("@Nombre", persona.Nombre);
                command.Parameters.AddWithValue("@Sexo", persona.sexo);
                command.Parameters.AddWithValue("@Edad", persona.edad);
                command.Parameters.AddWithValue("@Departamento", persona.departamento);
                command.Parameters.AddWithValue("@Ciudad", persona.ciudad);
                command.Parameters.AddWithValue("@Modalidad", persona.modalidad);
                command.Parameters.AddWithValue("@Aporte", persona.Aporte);
                command.Parameters.AddWithValue("@Fecha", persona.fecha);
                command.Parameters.AddWithValue("@TotalAporteEconomico", persona.TotalAporteEconomico);
                var filas = command.ExecuteNonQuery();
            }
        }

        public void GuardarValorTotal(int ValorTotal)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO Aportes(ValorTotal) VALUES @ValorTotal;";
                command.Parameters.AddWithValue("@ValorTotal", ValorTotal);
                var filas = command.ExecuteNonQuery();
            }
        }

        
        public void Modificar(Persona persona)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"UPDATE Personas SET Nombre = @Nombre, sexo = @Sexo, Edad = @Edad, Departamento = @Departamento,
                                      Ciudad = @Ciudad, Modalidad = @Modalidad, Aporte = @Aporte, Fecha = @Fecha, TotalAporteEconomico = @TotalAporteEconomico
                                      where Identificacion = @Identificacion";
                command.Parameters.AddWithValue("@Identificacion", persona.Identificacion);
                command.Parameters.AddWithValue("@Nombre", persona.Nombre);
                command.Parameters.AddWithValue("@Sexo", persona.sexo);
                command.Parameters.AddWithValue("@Edad", persona.edad);
                command.Parameters.AddWithValue("@Departamento", persona.departamento);
                command.Parameters.AddWithValue("@Ciudad", persona.ciudad);
                command.Parameters.AddWithValue("@Modalidad", persona.modalidad);
                command.Parameters.AddWithValue("@Aporte", persona.Aporte);
                command.Parameters.AddWithValue("@Fecha", persona.fecha);
                command.Parameters.AddWithValue("@TotalAporteEconomico", persona.TotalAporteEconomico);
                command.ExecuteNonQuery();
            }
            
        }

        public void Eliminar(Persona persona)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Delete from personas where Identificacion=@Identificacion";
                command.Parameters.AddWithValue("@Identificacion", persona.Identificacion);
                command.ExecuteNonQuery();
            }
        }
        public List<Persona> ConsultarTodos()
        {
            SqlDataReader dataReader;
            List<Persona> personas = new List<Persona>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Select * from personas ";
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Persona persona = DataReaderMapToPerson(dataReader);
                        personas.Add(persona);
                    }
                }
            }
            return personas;
        }
        
        public Persona BuscarPorIdentificacion(string identificacion)
        {
            SqlDataReader dataReader;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "select * from personas where Identificacion=@Identificacion";
                command.Parameters.AddWithValue("@Identificacion", identificacion);
                dataReader = command.ExecuteReader();
                dataReader.Read();
                return DataReaderMapToPerson(dataReader);
            }
        }
        private Persona DataReaderMapToPerson(SqlDataReader dataReader)
        {
            if(!dataReader.HasRows) return null;
            Persona persona = new Persona();
            persona.Identificacion = (string)dataReader["Identificacion"];
            persona.Nombre = (string)dataReader["Nombre"];
            persona.sexo = (string)dataReader["Sexo"];
            persona.edad = (int)dataReader["Edad"];
            persona.departamento = (string)dataReader["Departamento"];
            persona.ciudad  = (string)dataReader["Ciudad"];
            persona.modalidad = (string)dataReader["Modalidad"];
            persona.Aporte = (string)dataReader["Aporte"];
            persona.TotalAporteEconomico = (int)dataReader["TotalAporteEconomico"];
            return persona;
        }
        public int Totalizar()
        {
            return _personas.Count();
        }
        public int TotalizarMujeres()
        {
            ConsultarTodos();
            return _personas.Where(p => p.sexo.Equals("F")).Count();
        }
        public int TotalizarHombres()
        {
            ConsultarTodos();
            return _personas.Where(p => p.sexo.Equals("M")).Count();
        }
    }
}