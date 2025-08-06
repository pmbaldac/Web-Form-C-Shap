using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace TradePlace
{
    public class DBConect
    {
        string ConnectionDB = string.Empty;
        public DBConect()
        {
            ConnectionDB = ConfigurationManager.ConnectionStrings["SQLConn"].ToString();
        }
        public List<Community> GetCommunity()
        {
            using (SqlConnection conex = new SqlConnection(ConnectionDB))
            {
                try
                {
                    List<Community> lCommunity = new List<Community>();
                    StringBuilder sbQuery = new StringBuilder();
                    sbQuery.Append("SELECT TPCM_ID, LTRIM(TPCM_NAME) AS TPCM_NAME FROM [dbo].[TP_COMMUNITY] WHERE TPCMS_ID = 0 AND TPCMT_ID = 1 ORDER BY LTRIM(TPCM_NAME)");

                    using (SqlCommand cmd = new SqlCommand(sbQuery.ToString(), conex))
                    {
                        cmd.CommandType = CommandType.Text;
                        conex.Open();

                        using (SqlDataReader oReader = cmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                Community oCommunity = new Community
                                {
                                    Id = oReader["TPCM_ID"].ToString().Trim(),
                                    Name = oReader["TPCM_NAME"].ToString().Trim()
                                };

                                lCommunity.Add(oCommunity);
                            }
                        }

                        return lCommunity;
                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public List<Document> GetDocument()
        {
            using (SqlConnection conex = new SqlConnection(ConnectionDB))
            {
                try
                {
                    List<Document> lDocument = new List<Document>();
                    StringBuilder sbQuery = new StringBuilder();
                    sbQuery.Append("SELECT TPMO_ID, LTRIM(TPMO_NAME) AS TPMO_NAME FROM [dbo].[TP_MODULES_OPTIONS] WHERE TPMO_TYPE = 'D' ORDER BY TPMO_NAME");

                    using (SqlCommand cmd = new SqlCommand(sbQuery.ToString(), conex))
                    {
                        cmd.CommandType = CommandType.Text;
                        conex.Open();

                        using (SqlDataReader oReader = cmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                Document oDocument = new Document
                                {
                                    Id = oReader["TPMO_ID"].ToString().Trim(),
                                    Name = oReader["TPMO_NAME"].ToString().Trim()
                                };

                                lDocument.Add(oDocument);
                            }
                        }

                        return lDocument;
                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        public List<Template> GetTemplate(int iDocument)
        {
            using (SqlConnection conex = new SqlConnection(ConnectionDB))
            {
                try
                {
                    List<Template> lTemplate = new List<Template>();
                    StringBuilder sbQuery = new StringBuilder();
                    sbQuery.Append("SELECT TPMF_ID, LTRIM(TPMF_NAME) AS TPMF_NAME, LTRIM(TPMF_NAME_FIELD) AS TPMF_NAME_FIELD, TPMF_TYPE, TPMF_DATATYPE, TPMF_LENGTH, TPMF_LENGTH_DECIMAL, TPMF_REQUIRED, TPMF_ORDER, TPMF_TYPE_EXTRACTOR FROM TP_MODULES_TEMPLATE_FIELDS_EXTRACTOR WHERE TPMO_ID = ");
                    sbQuery.Append(iDocument);
                    sbQuery.Append(" ORDER BY TPMF_NAME");

                    using (SqlCommand cmd = new SqlCommand(sbQuery.ToString(), conex))
                    {
                        cmd.CommandType = CommandType.Text;
                        conex.Open();

                        using (SqlDataReader oReader = cmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                Template oTemplate = new Template
                                {
                                    Id = oReader["TPMF_ID"].ToString().Trim(),
                                    Name = oReader["TPMF_NAME"].ToString().Trim(),
                                    TableName = oReader["TPMF_NAME_FIELD"].ToString().Trim(),
                                    Type = oReader["TPMF_TYPE"].ToString().Trim(),
                                    DataType = oReader["TPMF_DATATYPE"].ToString().Trim(),
                                    Length = oReader["TPMF_LENGTH"].ToString().Trim(),
                                    LengthDecimal = oReader["TPMF_LENGTH_DECIMAL"].ToString().Trim(),
                                    Required = oReader["TPMF_REQUIRED"].ToString().Trim(),
                                    Order = oReader["TPMF_ORDER"].ToString().Trim(),
                                    Extractor = oReader["TPMF_TYPE_EXTRACTOR"].ToString().Trim()
                                };

                                lTemplate.Add(oTemplate);
                            }
                        }

                        return lTemplate;
                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        public List<Supplier> GetSupplier(int iCommunity, string sSupplier)
        {
            using (SqlConnection conex = new SqlConnection(ConnectionDB))
            {
                try
                {
                    List<Supplier> lSupplier = new List<Supplier>();
                    StringBuilder sbQuery = new StringBuilder();
                    sbQuery.Append("SELECT TPS_ID, TPS_NAME FROM [dbo].[TP_SUPPLIER] WHERE TPCM_ID = ");
                    sbQuery.Append(iCommunity);
                    sbQuery.Append(" AND TPS_NAME LIKE '%");
                    sbQuery.Append(sSupplier);
                    sbQuery.Append("%'");
                    sbQuery.Append(" ORDER BY TPS_NAME");

                    using (SqlCommand cmd = new SqlCommand(sbQuery.ToString(), conex))
                    {
                        cmd.CommandType = CommandType.Text;
                        conex.Open();

                        using (SqlDataReader oReader = cmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                Supplier oSupplier = new Supplier
                                {
                                    Id = oReader["TPS_ID"].ToString().Trim(),
                                    Name = oReader["TPS_NAME"].ToString().Trim()
                                };

                                lSupplier.Add(oSupplier);
                            }
                        }

                        return lSupplier;
                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public void SaveStructureExtractor(string sExtractionType, string sFieldName, string sFieldTableName, string sFieldType, string sDataType, int iLength, string sDecimalLength, int iFieldOrder, bool bIsRequired, int iDocumentId, int iCommunityId, int iSupplierId)
        {
            using (SqlConnection conex = new SqlConnection(ConnectionDB))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_SaveModuleStructureExtractor", conex))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ExtractionType", sExtractionType);
                        cmd.Parameters.AddWithValue("@FieldName", sFieldName);
                        cmd.Parameters.AddWithValue("@FieldTableName", sFieldTableName);
                        cmd.Parameters.AddWithValue("@FieldType", sFieldType);
                        cmd.Parameters.AddWithValue("@DataType", sDataType);
                        cmd.Parameters.AddWithValue("@Length", iLength);
                        if (sDecimalLength.Trim() == "")
                        {
                            cmd.Parameters.AddWithValue("@DecimalLength", null);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@DecimalLength", sDecimalLength);
                        }
                        cmd.Parameters.AddWithValue("@FieldOrder", iFieldOrder);
                        cmd.Parameters.AddWithValue("@IsRequired", bIsRequired);
                        cmd.Parameters.AddWithValue("@DocumentId", iDocumentId);
                        
                        if (iCommunityId == 0)
                        {
                            cmd.Parameters.AddWithValue("@CommunityId", null);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CommunityId", iCommunityId);
                        }
                        if (iSupplierId == 0)
                        {
                            cmd.Parameters.AddWithValue("@SupplierId", null);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@SupplierId", iSupplierId);
                        }
                        cmd.Parameters.AddWithValue("@UserId", 1);
                        cmd.Parameters.AddWithValue("@IsActive", 1);
                        conex.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        public void DeleteTemplate(string sExtractionType, string idField) 
        {
            using (SqlConnection conex = new SqlConnection(ConnectionDB))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteConfigurationExtractor", conex))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TipoExtract", sExtractionType);
                        cmd.Parameters.AddWithValue("@ConfigID", idField);
                        cmd.Parameters.AddWithValue("@TipoConfiguracion", "CE");
                        conex.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public void CopyCommunity(int iTPMO_ID, int iSOURCE_TPCM_ID, int iDESTINATION_TPCM_ID, string sTYPE_EXTRACTOR, out string sMensaje)
        {
            using (SqlConnection conex = new SqlConnection(ConnectionDB))
            {
                sMensaje = null;
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_COPY_MODULE_STRUCTURE_BETWEEN_COMMUNITIES", conex))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TPMO_ID", iTPMO_ID);
                        cmd.Parameters.AddWithValue("@SOURCE_TPCM_ID", iSOURCE_TPCM_ID);
                        cmd.Parameters.AddWithValue("@DESTINATION_TPCM_ID", iDESTINATION_TPCM_ID);
                        cmd.Parameters.AddWithValue("@TYPE_EXTRACTOR", sTYPE_EXTRACTOR);
                        cmd.Parameters.AddWithValue("@TPU_USER_ID", "1");
                        conex.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    sMensaje = ex.Message;
                }
            }
        }

        public List<Template> GetCopyCommunity(int iTPMO_ID, int iTPCM_ID, string sTYPE_EXTRACTOR)
        {
            using (SqlConnection conex = new SqlConnection(ConnectionDB))
            {
                try
                {
                    List<Template> lTemplate = new List<Template>();

                    using (SqlCommand cmd = new SqlCommand("SP_GET_MODULE_STRUCTURE_BY_COMMUNITY_OR_PROVIDER", conex))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TPMO_ID", iTPMO_ID);
                        cmd.Parameters.AddWithValue("@TPCM_ID", iTPCM_ID);
                        cmd.Parameters.AddWithValue("@TYPE_EXTRACTOR", sTYPE_EXTRACTOR);
                        conex.Open();

                        using (SqlDataReader oReader = cmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                Template oTemplate = new Template
                                {
                                    Id = oReader["TPMF_ID"].ToString().Trim(),
                                    Name = oReader["TPMS_NAME"].ToString().Trim(),
                                    TableName = oReader["TPMS_NAME_FIELD"].ToString().Trim(),
                                    Type = oReader["TPMS_TYPE"].ToString().Trim(),
                                    DataType = oReader["TPMS_DATATYPE"].ToString().Trim(),
                                    Length = oReader["TPMS_LENGTH"].ToString().Trim(),
                                    LengthDecimal = oReader["TPMS_DECIMAL_LENGTH"].ToString().Trim(),
                                    Required = oReader["TPMS_REQUIRED"].ToString().Trim(),
                                    Order = oReader["TPMS_ORDER"].ToString().Trim(),
                                    Extractor = oReader["TPMS_TYPE_EXTRACTOR"].ToString().Trim()
                                };

                                lTemplate.Add(oTemplate);
                            }
                        }

                        return lTemplate;
                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
    }
}