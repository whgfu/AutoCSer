﻿using System;

namespace AutoCSer.TestCase
{
    class Json
    {
        /// <summary>
        /// 随机对象生成参数
        /// </summary>
        internal static readonly AutoCSer.RandomObject.Config RandomConfig = new AutoCSer.RandomObject.Config { IsSecondDateTime = true, IsParseFloat = true };
        /// <summary>
        /// 带成员位图的JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.Json.SerializeConfig jsonSerializeConfig = new AutoCSer.Json.SerializeConfig();
        /// <summary>
        /// Javascript 时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.Json.SerializeConfig javascriptDateTimeSerializeConfig = new AutoCSer.Json.SerializeConfig { DateTimeType = AutoCSer.Json.DateTimeType.Javascript, IsBoolToInt = true, IsIntegerToHex = true };
        /// <summary>
        /// SQL 时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.Json.SerializeConfig sqlDateTimeSerializeConfig = new AutoCSer.Json.SerializeConfig { DateTimeType = AutoCSer.Json.DateTimeType.Sql };
        /// <summary>
        /// 第三方时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.Json.SerializeConfig thirdPartyDateTimeSerializeConfig = new AutoCSer.Json.SerializeConfig { DateTimeType = AutoCSer.Json.DateTimeType.ThirdParty };
        /// <summary>
        /// 自定义格式时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.Json.SerializeConfig customDateTimeSerializeConfig = new AutoCSer.Json.SerializeConfig { DateTimeType = AutoCSer.Json.DateTimeType.CustomFormat, DateTimeCustomFormat = "yyyy-MM-dd  HH:mm:ss" };

        /// <summary>
        /// JSON 序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            #region 引用类型字段成员JSON序列化测试
            Data.FieldData filedData = AutoCSer.RandomObject.Creator<Data.FieldData>.Create(RandomConfig);
            string jsonString = AutoCSer.JsonSerializer.Serialize(filedData);
            //AutoCSer.Log.Trace.Console(jsonString);
            Data.FieldData newFieldData = AutoCSer.JsonDeSerializer.DeSerialize<Data.FieldData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.FieldData>.Equals(filedData, newFieldData))
            {
                return false;
            }
            //AutoCSer.Log.Trace.Console(AutoCSer.Json.Serializer.Serialize(newFieldData));
            #endregion

            #region 带成员位图的引用类型字段成员JSON序列化测试
            jsonSerializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.FieldData>.NewFull();
            jsonString = AutoCSer.JsonSerializer.Serialize(filedData, jsonSerializeConfig);
            newFieldData = AutoCSer.JsonDeSerializer.DeSerialize<Data.FieldData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.FieldData>.MemberMapEquals(filedData, newFieldData, jsonSerializeConfig.MemberMap))
            {
                return false;
            }
            #endregion

            #region 引用类型属性成员JSON序列化测试
            Data.PropertyData propertyData = AutoCSer.RandomObject.Creator<Data.PropertyData>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(propertyData);
            Data.PropertyData newPropertyData = AutoCSer.JsonDeSerializer.DeSerialize<Data.PropertyData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.PropertyData>.Equals(propertyData, newPropertyData))
            {
                return false;
            }
            #endregion

            #region 值类型字段成员JSON序列化测试
            Data.StructFieldData structFieldData = AutoCSer.RandomObject.Creator<Data.StructFieldData>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(structFieldData);
            Data.StructFieldData newStructFieldData = AutoCSer.JsonDeSerializer.DeSerialize<Data.StructFieldData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructFieldData>.Equals(structFieldData, newStructFieldData))
            {
                return false;
            }
            #endregion

            #region 带成员位图的值类型字段成员JSON序列化测试
            jsonSerializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.StructFieldData>.NewFull();
            jsonString = AutoCSer.JsonSerializer.Serialize(structFieldData, jsonSerializeConfig);
            newStructFieldData = AutoCSer.JsonDeSerializer.DeSerialize<Data.StructFieldData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructFieldData>.MemberMapEquals(structFieldData, newStructFieldData, jsonSerializeConfig.MemberMap))
            {
                return false;
            }
            #endregion

            #region 值类型属性成员JSON序列化测试
            Data.StructPropertyData structPropertyData = AutoCSer.RandomObject.Creator<Data.StructPropertyData>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(structPropertyData);
            Data.StructPropertyData newStructPropertyData = AutoCSer.JsonDeSerializer.DeSerialize<Data.StructPropertyData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructPropertyData>.Equals(structPropertyData, newStructPropertyData))
            {
                return false;
            }
            #endregion

            #region 16进制整数JSON序列化测试
            filedData = AutoCSer.RandomObject.Creator<Data.FieldData>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(filedData, javascriptDateTimeSerializeConfig);
            newFieldData = AutoCSer.JsonDeSerializer.DeSerialize<Data.FieldData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.FieldData>.Equals(filedData, newFieldData))
            {
                return false;
            }
            #endregion

            #region 时间格式JSON序列化测试
            Data.MemberClass memberClassData = AutoCSer.RandomObject.Creator<Data.MemberClass>.Create(RandomConfig);
            memberClassData.DateTime = new DateTime(memberClassData.DateTime.Ticks, DateTimeKind.Local);
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData);
            Data.MemberClass newMemberClassData = AutoCSer.JsonDeSerializer.DeSerialize<Data.MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.MemberClass>.Equals(memberClassData, newMemberClassData))
            {
                return false;
            }
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData, sqlDateTimeSerializeConfig);
            newMemberClassData = AutoCSer.JsonDeSerializer.DeSerialize<Data.MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.MemberClass>.Equals(memberClassData, newMemberClassData))
            {
                return false;
            }
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData, thirdPartyDateTimeSerializeConfig);
            newMemberClassData = AutoCSer.JsonDeSerializer.DeSerialize<Data.MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.MemberClass>.Equals(memberClassData, newMemberClassData))
            {
                return false;
            }
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData, customDateTimeSerializeConfig);
            newMemberClassData = AutoCSer.JsonDeSerializer.DeSerialize<Data.MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.MemberClass>.Equals(memberClassData, newMemberClassData))
            {
                return false;
            }
            #endregion

            if (AutoCSer.JsonDeSerializer.DeSerialize<int>(jsonString = AutoCSer.JsonSerializer.Serialize<int>(1)) != 1)
            {
                return false;
            }
            if (AutoCSer.JsonDeSerializer.DeSerialize<string>(jsonString = AutoCSer.JsonSerializer.Serialize<string>("1")) != "1")
            {
                return false;
            }

            Data.Float floatData = AutoCSer.JsonDeSerializer.DeSerialize<Data.Float>(@"{Double4:-4.0,Double2:2.0,Double6:-6.0,Double5:5.0,Double3:-3.0}");
            if (floatData.Double2 != 2 || floatData.Double3 != -3 || floatData.Double4 != -4 || floatData.Double5 != 5 || floatData.Double6 != -6)
            {
                return false;
            }

            floatData = new Data.Float { FloatPositiveInfinity = float.NaN, FloatNegativeInfinity = float.NaN, DoublePositiveInfinity = double.NaN, DoubleNegativeInfinity = double.NaN };
            jsonString = AutoCSer.JsonSerializer.Serialize(floatData);
            Data.Float newFloatData = AutoCSer.JsonDeSerializer.DeSerialize<Data.Float>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.Float>.Equals(floatData, newFloatData))
            {
                return false;
            }

            floatData = new Data.Float();
            jsonString = AutoCSer.JsonSerializer.Serialize(floatData, new AutoCSer.Json.SerializeConfig { IsInfinityToNaN = false });
            newFloatData = AutoCSer.JsonDeSerializer.DeSerialize<Data.Float>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.Float>.Equals(floatData, newFloatData))
            {
                return false;
            }
            return true;
        }
    }
}
