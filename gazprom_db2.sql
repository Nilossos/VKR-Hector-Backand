PGDMP  -                    |            gazprom_db2    16.2    16.2 �    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16398    gazprom_db2    DATABASE        CREATE DATABASE gazprom_db2 WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
    DROP DATABASE gazprom_db2;
                postgres    false            �            1259    16399    CoefficientType    TABLE        CREATE TABLE public."CoefficientType" (
    "CoefficientTypeId" integer NOT NULL,
    "Name" character varying(30) NOT NULL
);
 %   DROP TABLE public."CoefficientType";
       public         heap    postgres    false            �            1259    16402 %   CoefficientType_CoefficientTypeId_seq    SEQUENCE     �   ALTER TABLE public."CoefficientType" ALTER COLUMN "CoefficientTypeId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."CoefficientType_CoefficientTypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    215            �            1259    16403    Company    TABLE     �   CREATE TABLE public."Company" (
    "CompanyId" integer NOT NULL,
    "PhoneNumber" character varying(20),
    "Email" character varying(50),
    "Url" character varying(200),
    "CompanyTypeId" integer
);
    DROP TABLE public."Company";
       public         heap    postgres    false            �            1259    16406    CompanyType    TABLE     x   CREATE TABLE public."CompanyType" (
    "CompanyTypeId" integer NOT NULL,
    "Name" character varying(100) NOT NULL
);
 !   DROP TABLE public."CompanyType";
       public         heap    postgres    false            �            1259    16409    CompanyType_CompanyTypeId_seq    SEQUENCE     �   ALTER TABLE public."CompanyType" ALTER COLUMN "CompanyTypeId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."CompanyType_CompanyTypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    218            �            1259    16410    Company_CompanyId_seq    SEQUENCE     �   ALTER TABLE public."Company" ALTER COLUMN "CompanyId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Company_CompanyId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    217            �            1259    16411    Construction    TABLE       CREATE TABLE public."Construction" (
    "ConstructionId" integer NOT NULL,
    "ObjectsId" integer,
    "ConstructionTypeId" integer,
    "ConstructionName" character varying(100) NOT NULL,
    "ConstructionStateId" integer NOT NULL,
    "BuildWayId" integer
);
 "   DROP TABLE public."Construction";
       public         heap    postgres    false            �            1259    16415    ConstructionState    TABLE     �   CREATE TABLE public."ConstructionState" (
    "ConstructionStateId" integer NOT NULL,
    "Name" character varying(50) NOT NULL
);
 '   DROP TABLE public."ConstructionState";
       public         heap    postgres    false            �            1259    16418 )   ConstructionState_ConstructionStateId_seq    SEQUENCE       ALTER TABLE public."ConstructionState" ALTER COLUMN "ConstructionStateId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ConstructionState_ConstructionStateId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    222            �            1259    16419    ConstructionType    TABLE     �   CREATE TABLE public."ConstructionType" (
    "ConstructionTypeId" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DocumentPath" character varying,
    "IsAssemblyShop" boolean NOT NULL
);
 &   DROP TABLE public."ConstructionType";
       public         heap    postgres    false            �            1259    16424    ConstructionUnit    TABLE     �   CREATE TABLE public."ConstructionUnit" (
    "ConstructionUnitId" integer NOT NULL,
    "ConstructionUnitTypeId" integer,
    "Name" character varying(100) NOT NULL,
    "MeasureUnitId" integer
);
 &   DROP TABLE public."ConstructionUnit";
       public         heap    postgres    false            �            1259    16427    ConstructionUnitType    TABLE     �   CREATE TABLE public."ConstructionUnitType" (
    "ConstructionUnitTypeId" integer NOT NULL,
    "Name" character varying(50) NOT NULL
);
 *   DROP TABLE public."ConstructionUnitType";
       public         heap    postgres    false            �            1259    16430 /   ConstructionUnitType_ConstructionUnitTypeId_seq    SEQUENCE       ALTER TABLE public."ConstructionUnitType" ALTER COLUMN "ConstructionUnitTypeId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ConstructionUnitType_ConstructionUnitTypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    226            �            1259    16431 '   ConstructionUnit_ConstructionUnitId_seq    SEQUENCE       ALTER TABLE public."ConstructionUnit" ALTER COLUMN "ConstructionUnitId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ConstructionUnit_ConstructionUnitId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    225            �            1259    16432    Construction_ConstructionId_seq    SEQUENCE     �   ALTER TABLE public."ConstructionType" ALTER COLUMN "ConstructionTypeId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Construction_ConstructionId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    224            �            1259    16433    DeliveryRegion    TABLE     �   CREATE TABLE public."DeliveryRegion" (
    "DeliveryRegionId" integer NOT NULL,
    "TransportFleet_TransportId" integer,
    "RegionId" integer
);
 $   DROP TABLE public."DeliveryRegion";
       public         heap    postgres    false            �            1259    16436 #   DeliveryRegion_DeliveryRegionId_seq    SEQUENCE     �   ALTER TABLE public."DeliveryRegion" ALTER COLUMN "DeliveryRegionId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."DeliveryRegion_DeliveryRegionId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    230            �            1259    16437    LogisticCompany    TABLE     �   CREATE TABLE public."LogisticCompany" (
    "LogisticCompanyId" integer NOT NULL,
    "Name" character varying(100) NOT NULL
);
 %   DROP TABLE public."LogisticCompany";
       public         heap    postgres    false            �            1259    16440    Manufacturer    TABLE     z   CREATE TABLE public."Manufacturer" (
    "ManufacturerId" integer NOT NULL,
    "Name" character varying(100) NOT NULL
);
 "   DROP TABLE public."Manufacturer";
       public         heap    postgres    false            �            1259    16443    MaterialSet    TABLE     n   CREATE TABLE public."MaterialSet" (
    "MaterialSetId" integer NOT NULL,
    "ConstructionTypeId" integer
);
 !   DROP TABLE public."MaterialSet";
       public         heap    postgres    false            �            1259    16446    MaterialSet_ConstructionUnit    TABLE     �   CREATE TABLE public."MaterialSet_ConstructionUnit" (
    "MaterialSet_ConstructionUnitId" integer NOT NULL,
    "MaterialSetId" integer,
    "ConstructionUnitId" integer,
    "Amount" real NOT NULL
);
 2   DROP TABLE public."MaterialSet_ConstructionUnit";
       public         heap    postgres    false            �            1259    16449 ?   MaterialSet_ConstructionUnit_MaterialSet_ConstructionUnitId_seq    SEQUENCE     1  ALTER TABLE public."MaterialSet_ConstructionUnit" ALTER COLUMN "MaterialSet_ConstructionUnitId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."MaterialSet_ConstructionUnit_MaterialSet_ConstructionUnitId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    235            �            1259    16450    MaterialSet_MaterialSetId_seq    SEQUENCE     �   ALTER TABLE public."MaterialSet" ALTER COLUMN "MaterialSetId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."MaterialSet_MaterialSetId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    234            �            1259    16451    MeasureUnit    TABLE     w   CREATE TABLE public."MeasureUnit" (
    "MeasureUnitId" integer NOT NULL,
    "Name" character varying(80) NOT NULL
);
 !   DROP TABLE public."MeasureUnit";
       public         heap    postgres    false            �            1259    16454    MeasureUnit_MeasureUnitId_seq    SEQUENCE     �   ALTER TABLE public."MeasureUnit" ALTER COLUMN "MeasureUnitId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."MeasureUnit_MeasureUnitId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    238            �            1259    16455    Mine    TABLE     �   CREATE TABLE public."Mine" (
    "MineId" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Coordinates" point NOT NULL,
    "DocumentPath" character varying,
    "SubsidiaryId" integer
);
    DROP TABLE public."Mine";
       public         heap    postgres    false            �            1259    16460    Mine_MineId_seq    SEQUENCE     �   ALTER TABLE public."Mine" ALTER COLUMN "MineId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Mine_MineId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    240            �            1259    16461    Objects    TABLE     �   CREATE TABLE public."Objects" (
    "ObjectsId" integer NOT NULL,
    "Name" character varying(100),
    "Coordinates" point NOT NULL,
    "MineId" integer NOT NULL,
    "RegionId" integer NOT NULL,
    "ContainsAssemblyShop" boolean NOT NULL
);
    DROP TABLE public."Objects";
       public         heap    postgres    false            �            1259    16464 /   Objects_Construction_Objects_ConstructionId_seq    SEQUENCE       ALTER TABLE public."Construction" ALTER COLUMN "ConstructionId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Objects_Construction_Objects_ConstructionId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    221            �            1259    16465    Objects_ObjectsId_seq    SEQUENCE     �   ALTER TABLE public."Objects" ALTER COLUMN "ObjectsId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Objects_ObjectsId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    242            �            1259    16466    Objects_TransportType    TABLE     �   CREATE TABLE public."Objects_TransportType" (
    "Objects_TransportTypeId" integer NOT NULL,
    "ObjectsId" integer,
    "TransportTypeId" integer
);
 +   DROP TABLE public."Objects_TransportType";
       public         heap    postgres    false            �            1259    16469 1   Objects_TransportType_Objects_TransportTypeId_seq    SEQUENCE       ALTER TABLE public."Objects_TransportType" ALTER COLUMN "Objects_TransportTypeId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Objects_TransportType_Objects_TransportTypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    245            �            1259    16470    Region    TABLE     m   CREATE TABLE public."Region" (
    "RegionId" integer NOT NULL,
    "Name" character varying(80) NOT NULL
);
    DROP TABLE public."Region";
       public         heap    postgres    false            �            1259    16473    Region_RegionId_seq    SEQUENCE     �   ALTER TABLE public."Region" ALTER COLUMN "RegionId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Region_RegionId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    247            �            1259    16474    Storage    TABLE     �   CREATE TABLE public."Storage" (
    "StorageId" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "RegionId" integer,
    "Address" character varying(200) NOT NULL,
    "ManufacturerId" integer,
    "Coordinates" point
);
    DROP TABLE public."Storage";
       public         heap    postgres    false            �            1259    16477    StorageToObjectsDistance    TABLE     �   CREATE TABLE public."StorageToObjectsDistance" (
    "StorageId" integer NOT NULL,
    "ObjectsId" integer NOT NULL,
    "Distance" numeric(8,3)
);
 .   DROP TABLE public."StorageToObjectsDistance";
       public         heap    postgres    false            �            1259    16480    StorageToTransportFleetDistance    TABLE     �   CREATE TABLE public."StorageToTransportFleetDistance" (
    "StorageId" integer NOT NULL,
    "TransportFleetId" integer NOT NULL,
    "Distance" numeric(8,3)
);
 5   DROP TABLE public."StorageToTransportFleetDistance";
       public         heap    postgres    false            �            1259    16483    Storage_ConstructionUnit    TABLE       CREATE TABLE public."Storage_ConstructionUnit" (
    "Storage_ConstructionUnitId" integer NOT NULL,
    "StorageId" integer,
    "ConstructionUnitId" integer,
    "PricePerUnit" numeric(13,2) NOT NULL,
    "DocumentPath" character varying,
    "TablePath" character varying
);
 .   DROP TABLE public."Storage_ConstructionUnit";
       public         heap    postgres    false            �            1259    16488 7   Storage_ConstructionUnit_Storage_ConstructionUnitId_seq    SEQUENCE     !  ALTER TABLE public."Storage_ConstructionUnit" ALTER COLUMN "Storage_ConstructionUnitId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Storage_ConstructionUnit_Storage_ConstructionUnitId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    252            �            1259    16489    Storage_StorageId_seq    SEQUENCE     �   ALTER TABLE public."Storage" ALTER COLUMN "StorageId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Storage_StorageId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    249            �            1259    16490 
   Subsidiary    TABLE     m   CREATE TABLE public."Subsidiary" (
    "SubsidiaryId" integer NOT NULL,
    "Name" character varying(100)
);
     DROP TABLE public."Subsidiary";
       public         heap    postgres    false                        1259    16493    Subsidiary_SubsidiaryId_seq    SEQUENCE     �   ALTER TABLE public."Subsidiary" ALTER COLUMN "SubsidiaryId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Subsidiary_SubsidiaryId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    255                       1259    16494 	   Transport    TABLE     �   CREATE TABLE public."Transport" (
    "TransportId" integer NOT NULL,
    "TransportModeId" integer,
    "Name" character varying(80) NOT NULL
);
    DROP TABLE public."Transport";
       public         heap    postgres    false                       1259    16497    TransportFleet    TABLE     �   CREATE TABLE public."TransportFleet" (
    "TransportFleetId" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Address" character varying(200) NOT NULL,
    "CompanyId" integer,
    "RegionId" integer,
    "Coordinates" point
);
 $   DROP TABLE public."TransportFleet";
       public         heap    postgres    false                       1259    16500    TransportFleetToObjectsDistance    TABLE     �   CREATE TABLE public."TransportFleetToObjectsDistance" (
    "TransportFleetId" integer NOT NULL,
    "ObjectsId" integer NOT NULL,
    "Distance" numeric(8,3)
);
 5   DROP TABLE public."TransportFleetToObjectsDistance";
       public         heap    postgres    false                       1259    16503    TransportFleet_Transport    TABLE       CREATE TABLE public."TransportFleet_Transport" (
    "TransportFleet_TransportId" integer NOT NULL,
    "TransportFleetId" integer,
    "TransportId" integer,
    "AverageSpeed" integer NOT NULL,
    "CoefficientTypeId" integer,
    "CoefficientValue" real NOT NULL
);
 .   DROP TABLE public."TransportFleet_Transport";
       public         heap    postgres    false                       1259    16506 #   TransportFleet_TransportFleetId_seq    SEQUENCE     �   ALTER TABLE public."TransportFleet" ALTER COLUMN "TransportFleetId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."TransportFleet_TransportFleetId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    258                       1259    16507 7   TransportFleet_Transport_TransportFleet_TransportId_seq    SEQUENCE     !  ALTER TABLE public."TransportFleet_Transport" ALTER COLUMN "TransportFleet_TransportId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."TransportFleet_Transport_TransportFleet_TransportId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    260                       1259    16508    TransportMode    TABLE     �   CREATE TABLE public."TransportMode" (
    "TransportModeId" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "TransportTypeId" integer
);
 #   DROP TABLE public."TransportMode";
       public         heap    postgres    false                       1259    16511 !   TransportMode_TransportModeId_seq    SEQUENCE     �   ALTER TABLE public."TransportMode" ALTER COLUMN "TransportModeId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."TransportMode_TransportModeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    263            	           1259    16512    TransportType    TABLE     |   CREATE TABLE public."TransportType" (
    "TransportTypeId" integer NOT NULL,
    "Name" character varying(100) NOT NULL
);
 #   DROP TABLE public."TransportType";
       public         heap    postgres    false            
           1259    16515 !   TransportType_TransportTypeId_seq    SEQUENCE     �   ALTER TABLE public."TransportType" ALTER COLUMN "TransportTypeId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."TransportType_TransportTypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    265                       1259    16516    Transport_TransportId_seq    SEQUENCE     �   ALTER TABLE public."Transport" ALTER COLUMN "TransportId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Transport_TransportId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    257                       1259    16517    User    TABLE     �  CREATE TABLE public."User" (
    "UserId" integer NOT NULL,
    "UserTypeId" integer,
    "SubsidiaryId" integer,
    "Surname" character varying(30) NOT NULL,
    "FirstName" character varying(30) NOT NULL,
    "Patronymic" character varying(30) NOT NULL,
    "Login" character varying(100) NOT NULL,
    "Password" character varying(64) NOT NULL,
    "PhoneNumber" character varying(20) NOT NULL,
    "BirthDate" date NOT NULL,
    "PhotoPath" character varying,
    "Token" character varying(300)
);
    DROP TABLE public."User";
       public         heap    postgres    false                       1259    16522    UserType    TABLE     h   CREATE TABLE public."UserType" (
    "UserTypeId" integer NOT NULL,
    "Name" character varying(30)
);
    DROP TABLE public."UserType";
       public         heap    postgres    false                       1259    16525    UserType_UserTypeId_seq    SEQUENCE     �   ALTER TABLE public."UserType" ALTER COLUMN "UserTypeId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."UserType_UserTypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    269                       1259    16526    User_UserId_seq    SEQUENCE     �   ALTER TABLE public."User" ALTER COLUMN "UserId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."User_UserId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    268            �          0    16399    CoefficientType 
   TABLE DATA           H   COPY public."CoefficientType" ("CoefficientTypeId", "Name") FROM stdin;
    public          postgres    false    215   �      �          0    16403    Company 
   TABLE DATA           `   COPY public."Company" ("CompanyId", "PhoneNumber", "Email", "Url", "CompanyTypeId") FROM stdin;
    public          postgres    false    217   �      �          0    16406    CompanyType 
   TABLE DATA           @   COPY public."CompanyType" ("CompanyTypeId", "Name") FROM stdin;
    public          postgres    false    218   j      �          0    16411    Construction 
   TABLE DATA           �   COPY public."Construction" ("ConstructionId", "ObjectsId", "ConstructionTypeId", "ConstructionName", "ConstructionStateId", "BuildWayId") FROM stdin;
    public          postgres    false    221   �      �          0    16415    ConstructionState 
   TABLE DATA           L   COPY public."ConstructionState" ("ConstructionStateId", "Name") FROM stdin;
    public          postgres    false    222   B      �          0    16419    ConstructionType 
   TABLE DATA           l   COPY public."ConstructionType" ("ConstructionTypeId", "Name", "DocumentPath", "IsAssemblyShop") FROM stdin;
    public          postgres    false    224   �      �          0    16424    ConstructionUnit 
   TABLE DATA           u   COPY public."ConstructionUnit" ("ConstructionUnitId", "ConstructionUnitTypeId", "Name", "MeasureUnitId") FROM stdin;
    public          postgres    false    225   k      �          0    16427    ConstructionUnitType 
   TABLE DATA           R   COPY public."ConstructionUnitType" ("ConstructionUnitTypeId", "Name") FROM stdin;
    public          postgres    false    226         �          0    16433    DeliveryRegion 
   TABLE DATA           h   COPY public."DeliveryRegion" ("DeliveryRegionId", "TransportFleet_TransportId", "RegionId") FROM stdin;
    public          postgres    false    230   W      �          0    16437    LogisticCompany 
   TABLE DATA           H   COPY public."LogisticCompany" ("LogisticCompanyId", "Name") FROM stdin;
    public          postgres    false    232   	      �          0    16440    Manufacturer 
   TABLE DATA           B   COPY public."Manufacturer" ("ManufacturerId", "Name") FROM stdin;
    public          postgres    false    233   l	      �          0    16443    MaterialSet 
   TABLE DATA           N   COPY public."MaterialSet" ("MaterialSetId", "ConstructionTypeId") FROM stdin;
    public          postgres    false    234   �	      �          0    16446    MaterialSet_ConstructionUnit 
   TABLE DATA           �   COPY public."MaterialSet_ConstructionUnit" ("MaterialSet_ConstructionUnitId", "MaterialSetId", "ConstructionUnitId", "Amount") FROM stdin;
    public          postgres    false    235   
      �          0    16451    MeasureUnit 
   TABLE DATA           @   COPY public."MeasureUnit" ("MeasureUnitId", "Name") FROM stdin;
    public          postgres    false    238         �          0    16455    Mine 
   TABLE DATA           a   COPY public."Mine" ("MineId", "Name", "Coordinates", "DocumentPath", "SubsidiaryId") FROM stdin;
    public          postgres    false    240   O      �          0    16461    Objects 
   TABLE DATA           u   COPY public."Objects" ("ObjectsId", "Name", "Coordinates", "MineId", "RegionId", "ContainsAssemblyShop") FROM stdin;
    public          postgres    false    242   �      �          0    16466    Objects_TransportType 
   TABLE DATA           l   COPY public."Objects_TransportType" ("Objects_TransportTypeId", "ObjectsId", "TransportTypeId") FROM stdin;
    public          postgres    false    245   �      �          0    16470    Region 
   TABLE DATA           6   COPY public."Region" ("RegionId", "Name") FROM stdin;
    public          postgres    false    247         �          0    16474    Storage 
   TABLE DATA           p   COPY public."Storage" ("StorageId", "Name", "RegionId", "Address", "ManufacturerId", "Coordinates") FROM stdin;
    public          postgres    false    249   O      �          0    16477    StorageToObjectsDistance 
   TABLE DATA           Z   COPY public."StorageToObjectsDistance" ("StorageId", "ObjectsId", "Distance") FROM stdin;
    public          postgres    false    250   R      �          0    16480    StorageToTransportFleetDistance 
   TABLE DATA           h   COPY public."StorageToTransportFleetDistance" ("StorageId", "TransportFleetId", "Distance") FROM stdin;
    public          postgres    false    251   R      �          0    16483    Storage_ConstructionUnit 
   TABLE DATA           �   COPY public."Storage_ConstructionUnit" ("Storage_ConstructionUnitId", "StorageId", "ConstructionUnitId", "PricePerUnit", "DocumentPath", "TablePath") FROM stdin;
    public          postgres    false    252   D      �          0    16490 
   Subsidiary 
   TABLE DATA           >   COPY public."Subsidiary" ("SubsidiaryId", "Name") FROM stdin;
    public          postgres    false    255   �      �          0    16494 	   Transport 
   TABLE DATA           O   COPY public."Transport" ("TransportId", "TransportModeId", "Name") FROM stdin;
    public          postgres    false    257         �          0    16497    TransportFleet 
   TABLE DATA           y   COPY public."TransportFleet" ("TransportFleetId", "Name", "Address", "CompanyId", "RegionId", "Coordinates") FROM stdin;
    public          postgres    false    258         �          0    16500    TransportFleetToObjectsDistance 
   TABLE DATA           h   COPY public."TransportFleetToObjectsDistance" ("TransportFleetId", "ObjectsId", "Distance") FROM stdin;
    public          postgres    false    259   �      �          0    16503    TransportFleet_Transport 
   TABLE DATA           �   COPY public."TransportFleet_Transport" ("TransportFleet_TransportId", "TransportFleetId", "TransportId", "AverageSpeed", "CoefficientTypeId", "CoefficientValue") FROM stdin;
    public          postgres    false    260   O      �          0    16508    TransportMode 
   TABLE DATA           W   COPY public."TransportMode" ("TransportModeId", "Name", "TransportTypeId") FROM stdin;
    public          postgres    false    263         �          0    16512    TransportType 
   TABLE DATA           D   COPY public."TransportType" ("TransportTypeId", "Name") FROM stdin;
    public          postgres    false    265   .       �          0    16517    User 
   TABLE DATA           �   COPY public."User" ("UserId", "UserTypeId", "SubsidiaryId", "Surname", "FirstName", "Patronymic", "Login", "Password", "PhoneNumber", "BirthDate", "PhotoPath", "Token") FROM stdin;
    public          postgres    false    268   u       �          0    16522    UserType 
   TABLE DATA           :   COPY public."UserType" ("UserTypeId", "Name") FROM stdin;
    public          postgres    false    269   �!      �           0    0 %   CoefficientType_CoefficientTypeId_seq    SEQUENCE SET     U   SELECT pg_catalog.setval('public."CoefficientType_CoefficientTypeId_seq"', 3, true);
          public          postgres    false    216            �           0    0    CompanyType_CompanyTypeId_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('public."CompanyType_CompanyTypeId_seq"', 2, true);
          public          postgres    false    219            �           0    0    Company_CompanyId_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."Company_CompanyId_seq"', 16, true);
          public          postgres    false    220            �           0    0 )   ConstructionState_ConstructionStateId_seq    SEQUENCE SET     Y   SELECT pg_catalog.setval('public."ConstructionState_ConstructionStateId_seq"', 4, true);
          public          postgres    false    223            �           0    0 /   ConstructionUnitType_ConstructionUnitTypeId_seq    SEQUENCE SET     _   SELECT pg_catalog.setval('public."ConstructionUnitType_ConstructionUnitTypeId_seq"', 2, true);
          public          postgres    false    227            �           0    0 '   ConstructionUnit_ConstructionUnitId_seq    SEQUENCE SET     X   SELECT pg_catalog.setval('public."ConstructionUnit_ConstructionUnitId_seq"', 22, true);
          public          postgres    false    228            �           0    0    Construction_ConstructionId_seq    SEQUENCE SET     O   SELECT pg_catalog.setval('public."Construction_ConstructionId_seq"', 8, true);
          public          postgres    false    229            �           0    0 #   DeliveryRegion_DeliveryRegionId_seq    SEQUENCE SET     T   SELECT pg_catalog.setval('public."DeliveryRegion_DeliveryRegionId_seq"', 48, true);
          public          postgres    false    231            �           0    0 ?   MaterialSet_ConstructionUnit_MaterialSet_ConstructionUnitId_seq    SEQUENCE SET     p   SELECT pg_catalog.setval('public."MaterialSet_ConstructionUnit_MaterialSet_ConstructionUnitId_seq"', 48, true);
          public          postgres    false    236            �           0    0    MaterialSet_MaterialSetId_seq    SEQUENCE SET     N   SELECT pg_catalog.setval('public."MaterialSet_MaterialSetId_seq"', 11, true);
          public          postgres    false    237            �           0    0    MeasureUnit_MeasureUnitId_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('public."MeasureUnit_MeasureUnitId_seq"', 4, true);
          public          postgres    false    239            �           0    0    Mine_MineId_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Mine_MineId_seq"', 5, true);
          public          postgres    false    241            �           0    0 /   Objects_Construction_Objects_ConstructionId_seq    SEQUENCE SET     `   SELECT pg_catalog.setval('public."Objects_Construction_Objects_ConstructionId_seq"', 32, true);
          public          postgres    false    243            �           0    0    Objects_ObjectsId_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."Objects_ObjectsId_seq"', 10, true);
          public          postgres    false    244            �           0    0 1   Objects_TransportType_Objects_TransportTypeId_seq    SEQUENCE SET     b   SELECT pg_catalog.setval('public."Objects_TransportType_Objects_TransportTypeId_seq"', 20, true);
          public          postgres    false    246            �           0    0    Region_RegionId_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public."Region_RegionId_seq"', 5, true);
          public          postgres    false    248            �           0    0 7   Storage_ConstructionUnit_Storage_ConstructionUnitId_seq    SEQUENCE SET     h   SELECT pg_catalog.setval('public."Storage_ConstructionUnit_Storage_ConstructionUnitId_seq"', 59, true);
          public          postgres    false    253            �           0    0    Storage_StorageId_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."Storage_StorageId_seq"', 10, true);
          public          postgres    false    254            �           0    0    Subsidiary_SubsidiaryId_seq    SEQUENCE SET     K   SELECT pg_catalog.setval('public."Subsidiary_SubsidiaryId_seq"', 1, true);
          public          postgres    false    256            �           0    0 #   TransportFleet_TransportFleetId_seq    SEQUENCE SET     T   SELECT pg_catalog.setval('public."TransportFleet_TransportFleetId_seq"', 16, true);
          public          postgres    false    261            �           0    0 7   TransportFleet_Transport_TransportFleet_TransportId_seq    SEQUENCE SET     h   SELECT pg_catalog.setval('public."TransportFleet_Transport_TransportFleet_TransportId_seq"', 48, true);
          public          postgres    false    262            �           0    0 !   TransportMode_TransportModeId_seq    SEQUENCE SET     Q   SELECT pg_catalog.setval('public."TransportMode_TransportModeId_seq"', 7, true);
          public          postgres    false    264            �           0    0 !   TransportType_TransportTypeId_seq    SEQUENCE SET     Q   SELECT pg_catalog.setval('public."TransportType_TransportTypeId_seq"', 3, true);
          public          postgres    false    266            �           0    0    Transport_TransportId_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public."Transport_TransportId_seq"', 14, true);
          public          postgres    false    267            �           0    0    UserType_UserTypeId_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public."UserType_UserTypeId_seq"', 4, true);
          public          postgres    false    270            �           0    0    User_UserId_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."User_UserId_seq"', 4, true);
          public          postgres    false    271            �           2606    16528 $   CoefficientType CoefficientType_pkey 
   CONSTRAINT     w   ALTER TABLE ONLY public."CoefficientType"
    ADD CONSTRAINT "CoefficientType_pkey" PRIMARY KEY ("CoefficientTypeId");
 R   ALTER TABLE ONLY public."CoefficientType" DROP CONSTRAINT "CoefficientType_pkey";
       public            postgres    false    215            �           2606    16530    CompanyType CompanyType_pkey 
   CONSTRAINT     k   ALTER TABLE ONLY public."CompanyType"
    ADD CONSTRAINT "CompanyType_pkey" PRIMARY KEY ("CompanyTypeId");
 J   ALTER TABLE ONLY public."CompanyType" DROP CONSTRAINT "CompanyType_pkey";
       public            postgres    false    218            �           2606    16532    Company Company_pkey 
   CONSTRAINT     _   ALTER TABLE ONLY public."Company"
    ADD CONSTRAINT "Company_pkey" PRIMARY KEY ("CompanyId");
 B   ALTER TABLE ONLY public."Company" DROP CONSTRAINT "Company_pkey";
       public            postgres    false    217            �           2606    16534 (   ConstructionState ConstructionState_pkey 
   CONSTRAINT     }   ALTER TABLE ONLY public."ConstructionState"
    ADD CONSTRAINT "ConstructionState_pkey" PRIMARY KEY ("ConstructionStateId");
 V   ALTER TABLE ONLY public."ConstructionState" DROP CONSTRAINT "ConstructionState_pkey";
       public            postgres    false    222            �           2606    16536 &   ConstructionType ConstructionType_pkey 
   CONSTRAINT     z   ALTER TABLE ONLY public."ConstructionType"
    ADD CONSTRAINT "ConstructionType_pkey" PRIMARY KEY ("ConstructionTypeId");
 T   ALTER TABLE ONLY public."ConstructionType" DROP CONSTRAINT "ConstructionType_pkey";
       public            postgres    false    224            �           2606    16538 .   ConstructionUnitType ConstructionUnitType_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public."ConstructionUnitType"
    ADD CONSTRAINT "ConstructionUnitType_pkey" PRIMARY KEY ("ConstructionUnitTypeId");
 \   ALTER TABLE ONLY public."ConstructionUnitType" DROP CONSTRAINT "ConstructionUnitType_pkey";
       public            postgres    false    226            �           2606    16540 &   ConstructionUnit ConstructionUnit_pkey 
   CONSTRAINT     z   ALTER TABLE ONLY public."ConstructionUnit"
    ADD CONSTRAINT "ConstructionUnit_pkey" PRIMARY KEY ("ConstructionUnitId");
 T   ALTER TABLE ONLY public."ConstructionUnit" DROP CONSTRAINT "ConstructionUnit_pkey";
       public            postgres    false    225            �           2606    16542    Construction Construction_pkey 
   CONSTRAINT     n   ALTER TABLE ONLY public."Construction"
    ADD CONSTRAINT "Construction_pkey" PRIMARY KEY ("ConstructionId");
 L   ALTER TABLE ONLY public."Construction" DROP CONSTRAINT "Construction_pkey";
       public            postgres    false    221            �           2606    16544 "   DeliveryRegion DeliveryRegion_pkey 
   CONSTRAINT     t   ALTER TABLE ONLY public."DeliveryRegion"
    ADD CONSTRAINT "DeliveryRegion_pkey" PRIMARY KEY ("DeliveryRegionId");
 P   ALTER TABLE ONLY public."DeliveryRegion" DROP CONSTRAINT "DeliveryRegion_pkey";
       public            postgres    false    230            �           2606    16546 $   LogisticCompany LogisticCompany_pkey 
   CONSTRAINT     w   ALTER TABLE ONLY public."LogisticCompany"
    ADD CONSTRAINT "LogisticCompany_pkey" PRIMARY KEY ("LogisticCompanyId");
 R   ALTER TABLE ONLY public."LogisticCompany" DROP CONSTRAINT "LogisticCompany_pkey";
       public            postgres    false    232            �           2606    16548    Manufacturer Manufacturer_pkey 
   CONSTRAINT     n   ALTER TABLE ONLY public."Manufacturer"
    ADD CONSTRAINT "Manufacturer_pkey" PRIMARY KEY ("ManufacturerId");
 L   ALTER TABLE ONLY public."Manufacturer" DROP CONSTRAINT "Manufacturer_pkey";
       public            postgres    false    233            �           2606    16550 >   MaterialSet_ConstructionUnit MaterialSet_ConstructionUnit_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public."MaterialSet_ConstructionUnit"
    ADD CONSTRAINT "MaterialSet_ConstructionUnit_pkey" PRIMARY KEY ("MaterialSet_ConstructionUnitId");
 l   ALTER TABLE ONLY public."MaterialSet_ConstructionUnit" DROP CONSTRAINT "MaterialSet_ConstructionUnit_pkey";
       public            postgres    false    235            �           2606    16552    MaterialSet MaterialSet_pkey 
   CONSTRAINT     k   ALTER TABLE ONLY public."MaterialSet"
    ADD CONSTRAINT "MaterialSet_pkey" PRIMARY KEY ("MaterialSetId");
 J   ALTER TABLE ONLY public."MaterialSet" DROP CONSTRAINT "MaterialSet_pkey";
       public            postgres    false    234            �           2606    16554    MeasureUnit MeasureUnit_pkey 
   CONSTRAINT     k   ALTER TABLE ONLY public."MeasureUnit"
    ADD CONSTRAINT "MeasureUnit_pkey" PRIMARY KEY ("MeasureUnitId");
 J   ALTER TABLE ONLY public."MeasureUnit" DROP CONSTRAINT "MeasureUnit_pkey";
       public            postgres    false    238            �           2606    16556    Mine Mine_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public."Mine"
    ADD CONSTRAINT "Mine_pkey" PRIMARY KEY ("MineId");
 <   ALTER TABLE ONLY public."Mine" DROP CONSTRAINT "Mine_pkey";
       public            postgres    false    240            �           2606    16558 0   Objects_TransportType Objects_TransportType_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public."Objects_TransportType"
    ADD CONSTRAINT "Objects_TransportType_pkey" PRIMARY KEY ("Objects_TransportTypeId");
 ^   ALTER TABLE ONLY public."Objects_TransportType" DROP CONSTRAINT "Objects_TransportType_pkey";
       public            postgres    false    245            �           2606    16560    Objects Objects_pkey 
   CONSTRAINT     _   ALTER TABLE ONLY public."Objects"
    ADD CONSTRAINT "Objects_pkey" PRIMARY KEY ("ObjectsId");
 B   ALTER TABLE ONLY public."Objects" DROP CONSTRAINT "Objects_pkey";
       public            postgres    false    242            �           2606    16562    Region Region_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public."Region"
    ADD CONSTRAINT "Region_pkey" PRIMARY KEY ("RegionId");
 @   ALTER TABLE ONLY public."Region" DROP CONSTRAINT "Region_pkey";
       public            postgres    false    247            �           2606    16564 6   StorageToObjectsDistance StorageToObjectsDistance_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public."StorageToObjectsDistance"
    ADD CONSTRAINT "StorageToObjectsDistance_pkey" PRIMARY KEY ("StorageId", "ObjectsId");
 d   ALTER TABLE ONLY public."StorageToObjectsDistance" DROP CONSTRAINT "StorageToObjectsDistance_pkey";
       public            postgres    false    250    250            �           2606    16566 D   StorageToTransportFleetDistance StorageToTransportFleetDistance_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public."StorageToTransportFleetDistance"
    ADD CONSTRAINT "StorageToTransportFleetDistance_pkey" PRIMARY KEY ("StorageId", "TransportFleetId");
 r   ALTER TABLE ONLY public."StorageToTransportFleetDistance" DROP CONSTRAINT "StorageToTransportFleetDistance_pkey";
       public            postgres    false    251    251            �           2606    16568 6   Storage_ConstructionUnit Storage_ConstructionUnit_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public."Storage_ConstructionUnit"
    ADD CONSTRAINT "Storage_ConstructionUnit_pkey" PRIMARY KEY ("Storage_ConstructionUnitId");
 d   ALTER TABLE ONLY public."Storage_ConstructionUnit" DROP CONSTRAINT "Storage_ConstructionUnit_pkey";
       public            postgres    false    252            �           2606    16570    Storage Storage_pkey 
   CONSTRAINT     _   ALTER TABLE ONLY public."Storage"
    ADD CONSTRAINT "Storage_pkey" PRIMARY KEY ("StorageId");
 B   ALTER TABLE ONLY public."Storage" DROP CONSTRAINT "Storage_pkey";
       public            postgres    false    249            �           2606    16572    Subsidiary Subsidiary_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public."Subsidiary"
    ADD CONSTRAINT "Subsidiary_pkey" PRIMARY KEY ("SubsidiaryId");
 H   ALTER TABLE ONLY public."Subsidiary" DROP CONSTRAINT "Subsidiary_pkey";
       public            postgres    false    255            �           2606    16574 D   TransportFleetToObjectsDistance TransportFleetToObjectsDistance_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public."TransportFleetToObjectsDistance"
    ADD CONSTRAINT "TransportFleetToObjectsDistance_pkey" PRIMARY KEY ("TransportFleetId", "ObjectsId");
 r   ALTER TABLE ONLY public."TransportFleetToObjectsDistance" DROP CONSTRAINT "TransportFleetToObjectsDistance_pkey";
       public            postgres    false    259    259            �           2606    16576 6   TransportFleet_Transport TransportFleet_Transport_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public."TransportFleet_Transport"
    ADD CONSTRAINT "TransportFleet_Transport_pkey" PRIMARY KEY ("TransportFleet_TransportId");
 d   ALTER TABLE ONLY public."TransportFleet_Transport" DROP CONSTRAINT "TransportFleet_Transport_pkey";
       public            postgres    false    260            �           2606    16578 "   TransportFleet TransportFleet_pkey 
   CONSTRAINT     t   ALTER TABLE ONLY public."TransportFleet"
    ADD CONSTRAINT "TransportFleet_pkey" PRIMARY KEY ("TransportFleetId");
 P   ALTER TABLE ONLY public."TransportFleet" DROP CONSTRAINT "TransportFleet_pkey";
       public            postgres    false    258            �           2606    16580     TransportMode TransportMode_pkey 
   CONSTRAINT     q   ALTER TABLE ONLY public."TransportMode"
    ADD CONSTRAINT "TransportMode_pkey" PRIMARY KEY ("TransportModeId");
 N   ALTER TABLE ONLY public."TransportMode" DROP CONSTRAINT "TransportMode_pkey";
       public            postgres    false    263            �           2606    16582     TransportType TransportType_pkey 
   CONSTRAINT     q   ALTER TABLE ONLY public."TransportType"
    ADD CONSTRAINT "TransportType_pkey" PRIMARY KEY ("TransportTypeId");
 N   ALTER TABLE ONLY public."TransportType" DROP CONSTRAINT "TransportType_pkey";
       public            postgres    false    265            �           2606    16584    Transport Transport_pkey 
   CONSTRAINT     e   ALTER TABLE ONLY public."Transport"
    ADD CONSTRAINT "Transport_pkey" PRIMARY KEY ("TransportId");
 F   ALTER TABLE ONLY public."Transport" DROP CONSTRAINT "Transport_pkey";
       public            postgres    false    257            �           2606    16586    UserType UserType_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public."UserType"
    ADD CONSTRAINT "UserType_pkey" PRIMARY KEY ("UserTypeId");
 D   ALTER TABLE ONLY public."UserType" DROP CONSTRAINT "UserType_pkey";
       public            postgres    false    269            �           2606    16587 "   Company Company_CompanyTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Company"
    ADD CONSTRAINT "Company_CompanyTypeId_fkey" FOREIGN KEY ("CompanyTypeId") REFERENCES public."CompanyType"("CompanyTypeId");
 P   ALTER TABLE ONLY public."Company" DROP CONSTRAINT "Company_CompanyTypeId_fkey";
       public          postgres    false    218    217    4784            �           2606    16592 =   ConstructionUnit ConstructionUnit_ConstructionUnitTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."ConstructionUnit"
    ADD CONSTRAINT "ConstructionUnit_ConstructionUnitTypeId_fkey" FOREIGN KEY ("ConstructionUnitTypeId") REFERENCES public."ConstructionUnitType"("ConstructionUnitTypeId");
 k   ALTER TABLE ONLY public."ConstructionUnit" DROP CONSTRAINT "ConstructionUnit_ConstructionUnitTypeId_fkey";
       public          postgres    false    225    4794    226            �           2606    16597 4   ConstructionUnit ConstructionUnit_MeasureUnitId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."ConstructionUnit"
    ADD CONSTRAINT "ConstructionUnit_MeasureUnitId_fkey" FOREIGN KEY ("MeasureUnitId") REFERENCES public."MeasureUnit"("MeasureUnitId") NOT VALID;
 b   ALTER TABLE ONLY public."ConstructionUnit" DROP CONSTRAINT "ConstructionUnit_MeasureUnitId_fkey";
       public          postgres    false    238    225    4806            �           2606    16602 2   Construction Construction_ConstructionStateId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Construction"
    ADD CONSTRAINT "Construction_ConstructionStateId_fkey" FOREIGN KEY ("ConstructionStateId") REFERENCES public."ConstructionState"("ConstructionStateId");
 `   ALTER TABLE ONLY public."Construction" DROP CONSTRAINT "Construction_ConstructionStateId_fkey";
       public          postgres    false    221    222    4788            �           2606    16607 1   Construction Construction_ConstructionTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Construction"
    ADD CONSTRAINT "Construction_ConstructionTypeId_fkey" FOREIGN KEY ("ConstructionTypeId") REFERENCES public."ConstructionType"("ConstructionTypeId");
 _   ALTER TABLE ONLY public."Construction" DROP CONSTRAINT "Construction_ConstructionTypeId_fkey";
       public          postgres    false    224    4790    221            �           2606    16612 (   Construction Construction_ObjectsId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Construction"
    ADD CONSTRAINT "Construction_ObjectsId_fkey" FOREIGN KEY ("ObjectsId") REFERENCES public."Objects"("ObjectsId");
 V   ALTER TABLE ONLY public."Construction" DROP CONSTRAINT "Construction_ObjectsId_fkey";
       public          postgres    false    221    242    4810            �           2606    16617 +   DeliveryRegion DeliveryRegion_RegionId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."DeliveryRegion"
    ADD CONSTRAINT "DeliveryRegion_RegionId_fkey" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 Y   ALTER TABLE ONLY public."DeliveryRegion" DROP CONSTRAINT "DeliveryRegion_RegionId_fkey";
       public          postgres    false    230    247    4814            �           2606    16622 =   DeliveryRegion DeliveryRegion_TransportFleet_TransportId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."DeliveryRegion"
    ADD CONSTRAINT "DeliveryRegion_TransportFleet_TransportId_fkey" FOREIGN KEY ("TransportFleet_TransportId") REFERENCES public."TransportFleet_Transport"("TransportFleet_TransportId");
 k   ALTER TABLE ONLY public."DeliveryRegion" DROP CONSTRAINT "DeliveryRegion_TransportFleet_TransportId_fkey";
       public          postgres    false    230    260    4832            �           2606    16627 6   LogisticCompany LogisticCompany_LogisticCompanyId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."LogisticCompany"
    ADD CONSTRAINT "LogisticCompany_LogisticCompanyId_fkey" FOREIGN KEY ("LogisticCompanyId") REFERENCES public."Company"("CompanyId");
 d   ALTER TABLE ONLY public."LogisticCompany" DROP CONSTRAINT "LogisticCompany_LogisticCompanyId_fkey";
       public          postgres    false    232    217    4782            �           2606    16632 -   Manufacturer Manufacturer_ManufacturerId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Manufacturer"
    ADD CONSTRAINT "Manufacturer_ManufacturerId_fkey" FOREIGN KEY ("ManufacturerId") REFERENCES public."Company"("CompanyId");
 [   ALTER TABLE ONLY public."Manufacturer" DROP CONSTRAINT "Manufacturer_ManufacturerId_fkey";
       public          postgres    false    233    4782    217            �           2606    16637 /   MaterialSet MaterialSet_ConstructionTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."MaterialSet"
    ADD CONSTRAINT "MaterialSet_ConstructionTypeId_fkey" FOREIGN KEY ("ConstructionTypeId") REFERENCES public."ConstructionType"("ConstructionTypeId");
 ]   ALTER TABLE ONLY public."MaterialSet" DROP CONSTRAINT "MaterialSet_ConstructionTypeId_fkey";
       public          postgres    false    4790    224    234            �           2606    16642 Q   MaterialSet_ConstructionUnit MaterialSet_ConstructionUnit_ConstructionUnitId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."MaterialSet_ConstructionUnit"
    ADD CONSTRAINT "MaterialSet_ConstructionUnit_ConstructionUnitId_fkey" FOREIGN KEY ("ConstructionUnitId") REFERENCES public."ConstructionUnit"("ConstructionUnitId");
    ALTER TABLE ONLY public."MaterialSet_ConstructionUnit" DROP CONSTRAINT "MaterialSet_ConstructionUnit_ConstructionUnitId_fkey";
       public          postgres    false    225    235    4792            �           2606    16647 L   MaterialSet_ConstructionUnit MaterialSet_ConstructionUnit_MaterialSetId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."MaterialSet_ConstructionUnit"
    ADD CONSTRAINT "MaterialSet_ConstructionUnit_MaterialSetId_fkey" FOREIGN KEY ("MaterialSetId") REFERENCES public."MaterialSet"("MaterialSetId");
 z   ALTER TABLE ONLY public."MaterialSet_ConstructionUnit" DROP CONSTRAINT "MaterialSet_ConstructionUnit_MaterialSetId_fkey";
       public          postgres    false    234    235    4802            �           2606    16652    Mine Mine_SubsidiaryId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Mine"
    ADD CONSTRAINT "Mine_SubsidiaryId_fkey" FOREIGN KEY ("SubsidiaryId") REFERENCES public."Subsidiary"("SubsidiaryId");
 I   ALTER TABLE ONLY public."Mine" DROP CONSTRAINT "Mine_SubsidiaryId_fkey";
       public          postgres    false    4824    255    240            �           2606    16657    Objects Objects_MineId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Objects"
    ADD CONSTRAINT "Objects_MineId_fkey" FOREIGN KEY ("MineId") REFERENCES public."Mine"("MineId");
 I   ALTER TABLE ONLY public."Objects" DROP CONSTRAINT "Objects_MineId_fkey";
       public          postgres    false    242    240    4808            �           2606    16662    Objects Objects_RegionId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Objects"
    ADD CONSTRAINT "Objects_RegionId_fkey" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 K   ALTER TABLE ONLY public."Objects" DROP CONSTRAINT "Objects_RegionId_fkey";
       public          postgres    false    247    242    4814            �           2606    16667 :   Objects_TransportType Objects_TransportType_ObjectsId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Objects_TransportType"
    ADD CONSTRAINT "Objects_TransportType_ObjectsId_fkey" FOREIGN KEY ("ObjectsId") REFERENCES public."Objects"("ObjectsId");
 h   ALTER TABLE ONLY public."Objects_TransportType" DROP CONSTRAINT "Objects_TransportType_ObjectsId_fkey";
       public          postgres    false    245    242    4810            �           2606    16672 @   Objects_TransportType Objects_TransportType_TransportTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Objects_TransportType"
    ADD CONSTRAINT "Objects_TransportType_TransportTypeId_fkey" FOREIGN KEY ("TransportTypeId") REFERENCES public."TransportType"("TransportTypeId");
 n   ALTER TABLE ONLY public."Objects_TransportType" DROP CONSTRAINT "Objects_TransportType_TransportTypeId_fkey";
       public          postgres    false    265    4836    245            �           2606    16677 @   StorageToObjectsDistance StorageToObjectsDistance_ObjectsId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."StorageToObjectsDistance"
    ADD CONSTRAINT "StorageToObjectsDistance_ObjectsId_fkey" FOREIGN KEY ("ObjectsId") REFERENCES public."Objects"("ObjectsId");
 n   ALTER TABLE ONLY public."StorageToObjectsDistance" DROP CONSTRAINT "StorageToObjectsDistance_ObjectsId_fkey";
       public          postgres    false    242    250    4810            �           2606    16682 @   StorageToObjectsDistance StorageToObjectsDistance_StorageId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."StorageToObjectsDistance"
    ADD CONSTRAINT "StorageToObjectsDistance_StorageId_fkey" FOREIGN KEY ("StorageId") REFERENCES public."Storage"("StorageId");
 n   ALTER TABLE ONLY public."StorageToObjectsDistance" DROP CONSTRAINT "StorageToObjectsDistance_StorageId_fkey";
       public          postgres    false    249    250    4816            �           2606    16687 N   StorageToTransportFleetDistance StorageToTransportFleetDistance_StorageId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."StorageToTransportFleetDistance"
    ADD CONSTRAINT "StorageToTransportFleetDistance_StorageId_fkey" FOREIGN KEY ("StorageId") REFERENCES public."Storage"("StorageId");
 |   ALTER TABLE ONLY public."StorageToTransportFleetDistance" DROP CONSTRAINT "StorageToTransportFleetDistance_StorageId_fkey";
       public          postgres    false    4816    249    251            �           2606    16692 U   StorageToTransportFleetDistance StorageToTransportFleetDistance_TransportFleetId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."StorageToTransportFleetDistance"
    ADD CONSTRAINT "StorageToTransportFleetDistance_TransportFleetId_fkey" FOREIGN KEY ("TransportFleetId") REFERENCES public."TransportFleet"("TransportFleetId");
 �   ALTER TABLE ONLY public."StorageToTransportFleetDistance" DROP CONSTRAINT "StorageToTransportFleetDistance_TransportFleetId_fkey";
       public          postgres    false    251    258    4828            �           2606    16697 I   Storage_ConstructionUnit Storage_ConstructionUnit_ConstructionUnitId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Storage_ConstructionUnit"
    ADD CONSTRAINT "Storage_ConstructionUnit_ConstructionUnitId_fkey" FOREIGN KEY ("ConstructionUnitId") REFERENCES public."ConstructionUnit"("ConstructionUnitId");
 w   ALTER TABLE ONLY public."Storage_ConstructionUnit" DROP CONSTRAINT "Storage_ConstructionUnit_ConstructionUnitId_fkey";
       public          postgres    false    4792    225    252                        2606    16702 @   Storage_ConstructionUnit Storage_ConstructionUnit_StorageId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Storage_ConstructionUnit"
    ADD CONSTRAINT "Storage_ConstructionUnit_StorageId_fkey" FOREIGN KEY ("StorageId") REFERENCES public."Storage"("StorageId");
 n   ALTER TABLE ONLY public."Storage_ConstructionUnit" DROP CONSTRAINT "Storage_ConstructionUnit_StorageId_fkey";
       public          postgres    false    4816    252    249            �           2606    16707 #   Storage Storage_ManufacturerId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Storage"
    ADD CONSTRAINT "Storage_ManufacturerId_fkey" FOREIGN KEY ("ManufacturerId") REFERENCES public."Manufacturer"("ManufacturerId");
 Q   ALTER TABLE ONLY public."Storage" DROP CONSTRAINT "Storage_ManufacturerId_fkey";
       public          postgres    false    4800    249    233            �           2606    16712    Storage Storage_RegionId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Storage"
    ADD CONSTRAINT "Storage_RegionId_fkey" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 K   ALTER TABLE ONLY public."Storage" DROP CONSTRAINT "Storage_RegionId_fkey";
       public          postgres    false    4814    249    247                       2606    16717 N   TransportFleetToObjectsDistance TransportFleetToObjectsDistance_ObjectsId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."TransportFleetToObjectsDistance"
    ADD CONSTRAINT "TransportFleetToObjectsDistance_ObjectsId_fkey" FOREIGN KEY ("ObjectsId") REFERENCES public."Objects"("ObjectsId");
 |   ALTER TABLE ONLY public."TransportFleetToObjectsDistance" DROP CONSTRAINT "TransportFleetToObjectsDistance_ObjectsId_fkey";
       public          postgres    false    4810    242    259                       2606    16722 U   TransportFleetToObjectsDistance TransportFleetToObjectsDistance_TransportFleetId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."TransportFleetToObjectsDistance"
    ADD CONSTRAINT "TransportFleetToObjectsDistance_TransportFleetId_fkey" FOREIGN KEY ("TransportFleetId") REFERENCES public."TransportFleet"("TransportFleetId");
 �   ALTER TABLE ONLY public."TransportFleetToObjectsDistance" DROP CONSTRAINT "TransportFleetToObjectsDistance_TransportFleetId_fkey";
       public          postgres    false    258    4828    259                       2606    16727 ,   TransportFleet TransportFleet_CompanyId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."TransportFleet"
    ADD CONSTRAINT "TransportFleet_CompanyId_fkey" FOREIGN KEY ("CompanyId") REFERENCES public."Company"("CompanyId");
 Z   ALTER TABLE ONLY public."TransportFleet" DROP CONSTRAINT "TransportFleet_CompanyId_fkey";
       public          postgres    false    4782    217    258                       2606    16732 +   TransportFleet TransportFleet_RegionId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."TransportFleet"
    ADD CONSTRAINT "TransportFleet_RegionId_fkey" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 Y   ALTER TABLE ONLY public."TransportFleet" DROP CONSTRAINT "TransportFleet_RegionId_fkey";
       public          postgres    false    258    247    4814                       2606    16737 H   TransportFleet_Transport TransportFleet_Transport_CoefficientTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."TransportFleet_Transport"
    ADD CONSTRAINT "TransportFleet_Transport_CoefficientTypeId_fkey" FOREIGN KEY ("CoefficientTypeId") REFERENCES public."CoefficientType"("CoefficientTypeId");
 v   ALTER TABLE ONLY public."TransportFleet_Transport" DROP CONSTRAINT "TransportFleet_Transport_CoefficientTypeId_fkey";
       public          postgres    false    260    215    4780                       2606    16742 G   TransportFleet_Transport TransportFleet_Transport_TransportFleetId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."TransportFleet_Transport"
    ADD CONSTRAINT "TransportFleet_Transport_TransportFleetId_fkey" FOREIGN KEY ("TransportFleetId") REFERENCES public."TransportFleet"("TransportFleetId");
 u   ALTER TABLE ONLY public."TransportFleet_Transport" DROP CONSTRAINT "TransportFleet_Transport_TransportFleetId_fkey";
       public          postgres    false    258    260    4828                       2606    16747 B   TransportFleet_Transport TransportFleet_Transport_TransportId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."TransportFleet_Transport"
    ADD CONSTRAINT "TransportFleet_Transport_TransportId_fkey" FOREIGN KEY ("TransportId") REFERENCES public."Transport"("TransportId");
 p   ALTER TABLE ONLY public."TransportFleet_Transport" DROP CONSTRAINT "TransportFleet_Transport_TransportId_fkey";
       public          postgres    false    4826    257    260            	           2606    16752 0   TransportMode TransportMode_TransportTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."TransportMode"
    ADD CONSTRAINT "TransportMode_TransportTypeId_fkey" FOREIGN KEY ("TransportTypeId") REFERENCES public."TransportType"("TransportTypeId");
 ^   ALTER TABLE ONLY public."TransportMode" DROP CONSTRAINT "TransportMode_TransportTypeId_fkey";
       public          postgres    false    4836    263    265                       2606    16757 (   Transport Transport_TransportModeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Transport"
    ADD CONSTRAINT "Transport_TransportModeId_fkey" FOREIGN KEY ("TransportModeId") REFERENCES public."TransportMode"("TransportModeId");
 V   ALTER TABLE ONLY public."Transport" DROP CONSTRAINT "Transport_TransportModeId_fkey";
       public          postgres    false    263    4834    257            
           2606    16762    User User_SubsidiaryId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_SubsidiaryId_fkey" FOREIGN KEY ("SubsidiaryId") REFERENCES public."Subsidiary"("SubsidiaryId");
 I   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_SubsidiaryId_fkey";
       public          postgres    false    255    268    4824                       2606    16767    User User_UserTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_UserTypeId_fkey" FOREIGN KEY ("UserTypeId") REFERENCES public."UserType"("UserTypeId");
 G   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_UserTypeId_fkey";
       public          postgres    false    268    269    4838            �   ,   x�3��p���F��.��2��.6q�� )��M�\1z\\\ �X�      �   r  x����n� �3>�&[GD��>H/��.)�A�ݷ/t0�4i�5���3 yt��B�h�;Af��7��o��F]F��{����z�%���Vipq~&Mՠ�С��-#j�������+��L1� ��!�����Z��������+Y����6��ʲ���Hq�9�]�mP���0<32B�>qk�˾���ˌ�e�Nդp�X5�Yy�WY��'9��+�7U��rR2����c���j�5�դvR<��g<y�ò5B,�C�85ދ��� ���u�	�;���㧅��O�7U�t���ݳ�˺iW�H����ٷ��=^��fsf�٭��O��g�Fߔ�I���tp�ax�KrBi�Pg��RU���x      �   T   x���	�0 �s�
+��1^�)��-�h0��-�v��t�MG*��d=(:��u�ޱ�Fް�LV�E��X*��V���t�3}      �   d  x��RQN�@��=E/��l[�~�<��c"� ~�ߕ�V��
�7��P`Q4��e������SB�Dr'�pƲ�w��ۮ�Q�07���6\K-/�<�`!�,���z�;=�n�@ޥ�d a�4\��U�D�3��7+��"`]VJ�nS�������<L�C[ʫ	o� x���D5�\le=�u�Q�\K�3v%��l���6��$G�=q��6�Kb��u�Q���ƍ�@����[���C� ���x�f`�ѡ��ß<!7�6Ss*a��j��%�)	A���'v��c'
��n3�V��o֘�g.W>��	\ �0�.�`���H�2��6��e�YC#$�ڬ�T�'ιF&      �   T   x�3�0��/��{/��pa߅M`�>.#��.l
����mh�Ɯ^l� ҍ��L8/̿�Ȅo������ Y�B      �   �   x�e���0D�v� �?UPA�T@��!Q W�&"`�0-�vĮP"��}3���	m��
/���P��+#�.�J�~B4xS��k���W���#�Khǉ�w�kf��H�
G*�e�[I9�����
g���3��R�-r[��|ޗ{.~-���B�̒����`�u:�Z �a��      �   �  x��S�Q�0���pL� �PL^�@2t"'8/�����=ٌ���x�{����.s�Nt*^�"'�.��U��3��/+��
dk� ���,�׼nQ�k��)��}�%�K D�#üױ�����c\	z{��T�M��X�Z��]w�<���Q726\V૑���|�I�9B���:3�蓱��Qh�a8iX爿��Y�余E_�@�D�o���Hc�A�$[��o������fîRl�8j=���D��+q���
�N��
,�vX�E;s�s�E@$q	��(�g	|�o���©�qO�C�=�,���B���Ǉ���9>��?���6��S�k\�@�+}Fv+����<�ҿ��J^�bً���$�|�m+��/f;^�C������<�)�L      �   @   x�3估�¾��@�}a��.#�/�
�_�{a��~�L�ņ��v��6p��qqq �!�      �   �   x��ɍD1�3��k�K���u�S��R��fG�.-vk�G�����c�W�<N����1.�ra�t\��=u�炥V.X�`�-l�`����'n��>,]�r��l�`�;Z�W�s��炥V.X�`�-l�Gco����_D���2�      �   W   x�3�0�¾�/��x�	H�_�z��®.�+ �}�\�aÅ�@�~C.#��q��ޘ˄$�&\�$�7�2#I�W� F��      �   a   x��л�0C�O�(�?�0�fv@ "!1��Fd�G�[��i��-�J����;��U.y�6YE4�� u@��:]$��"�A�?�����      �   2   x���  ���s$���A��e�S����&���íw��k^>��W      �   �   x�5��u�0Ϣ�<	Я��_Gv��|�H���6xz7�����tKQMѤ����ܢ]���\x�S8zCtHs���p��!ΏS|�O:��aq8�a�[���=�+L5G��5	�b\%E�S-)�x���$M���H�n��؏�K��m����q�
')�$I��B[��xR]�K	O��f�%$��o�����J�?��7�eP�߿L�i甉�1,+��V�]+�L����� ��L�      �   $   x�3��q��ˈ�®���9��	��\1z\\\ �	
�      �   �   x�}α1F�:���Ȓ�߉o	&�a�9� ng#RWf�O�q�~����7�˛����ᬠ,Z�X��^G�)J���#C-��3��*�dAh��]a�0T��,	BV��ҙҡ�D0GӎÕb�?C�x�      �   �   x�]б1��:����N�ٱ{�L���4Lql�QrE��~ų)���������(�U0sYE��lI5���p��0�0T��uC��K⨁�P$�к���N��̾����%�A�a9`w·����*�X�hUJt���ـ�a��4d����~7kԀ��wo�f�zA �g��      �   R   x��� 1�ji�Cl��%��q`�PA�B�T�R�����pi�����wb�#)Q��N�1�th��B�P9�������k      �   3   x�3估����/츰��^C.#T#.cTc.T.STS�=... ,�1-      �   �  x����n�@���O�K,�������	*�\(���
�������0~#ά�q� �paɞ=;盝Yŗ|�k��Mm�Q��W��7��X���k|\���������o��ު~�K칗=*�g>u>'J-�.d��6�������_����Dx�e��7Cl�[�;��Rj*7�p
L���Q�WR�g$����OQ��]��+����?���%����R�Sș���t(�9t�R�����B�V�)b+�Dp���z�%��A/��+����;��b�.�4��w�	��0�ZI�1������R�'�a���P��a��]���L,�b��c�2E��@VZ�8Q�_Z�A���G�_x��r����39⏒��cͰ���+ �� ��i�4�O���N��&�0��y�e]^�*�kV���)$�owrg����@�C�Z�Q�ӱ�0��?�>��?�"��W|(��}Үu	��G��yWU�/,��J      �   �  x�M��q� �5.F�?R/鿎\Af�ds�g��-K�'f�d)l�[a[�AY���KH=�|�Wh���_�{�lvX�\��ۦ���#8��\��n�c����I�������O� C�ka��1�����n0�U���{e:��{�q�6U�$�-�?Gl2�c���c!�����p�t�ֹ'�U�TX�#�߽R���	�Bʿ@����	�5��Zb��1��fs?~����{yI��q/���d�-D��`M�X�m<�J��	V2t)'X��ct��L��zܧ�9Y|�oO��ф�X{�`G���~\9�q.']}<3�x��f��u��*_��V�ъct�D������YN���P�%;�z��BR{M�{��rh�h��Q�*W�x��e���*Xچ�1Nk;%^�F��:��1������%�v�u
=E�)�&�P�7 K=E����td��-С�5�]oq��o�7݆��}���T�s      �   �  x�-�I�$!��c0@�e����P�Y��\[�{�����q�G�Dظ~�9�����櫋�a�bz"�=�ӟvj��i�Y�QY�>zc�u�.�ר��[:ϴ,)�ӧ�p[k��{>��̕O*�zs������a;\;'B�{�[!Ō�u#�<�+�tf�A4�M�D�t֌��*�^���N��~�����F�7�S��0�dAH�մ�=!ٛ�m��lޔߍ����<F^�����`k�<RT�!@2�3H�	���4(,OO��+H���ȟ t���y[�Ari[W����r��Ay#�X5�@�ًV -T���/�Q��Zo&�E�b�r�� ֹ� ����c Օ�7ҵ�(�G@��R@��!*�?Գ7Q탱-��֍ҵ�� Z�)�'����ؠ8��q�J��JB!3 �A@T��3�y-߈Tm����¸�Bw�i`��o��ɕ�$L�UGHI��D?`z�)�*+	8JW�	)�MjM*I.��|)���}��D��k�����7�������;Br���D��l�K�u�����^�9cd,��.���Ii��FZ �l7͢��������a�Wj�R�;k5��h�m5�%�D��,��nt���N|!$���;������$�{$P�v��9O����U#��.�?$F����7��[�JN�+$��v�ʙ Bb���4��uE��̪1n��ES# Դ{,����{YC$<��f����ӎW�<R$�o+��k��A��/)�q2�D�>��g>Fʃ���bZk���L��P�d~z4�Z� �PM;d_/%�����PK�yƂ�H[c�Ή��(fʂ��+\ӻLF�R�}����k���ﶢJ��}�O���$��%����as)�����^b��ݲ6�9�=
-�E������j%?Q/�5�9�׷N�-75ڌ��V:�o H��VK�@��(B�����R�1/���o~����\�      �   o  x�e�ɱ�0C�t1���&~鿎 ���X��q��t<�Z{�&�?�GAL�0�K�`��ؒ(l��F��&jE�H�*7��*����uG2�`�������ӱˬ4H�r�[��㚨��5���'܄𘗇6l�?Y+{��q�E��<��9�K"^X�[w��0�8���b�}>ڎ��7�ٽS#���:���b�8'����0�����dm[7k�6ѳ%zE�@�5��s�H��xc�Q`=a��v�A�]�r#���:ÂG�
��u����0��61����`�v�Mx��W�����R՚�$�ʍ�)�s��~_���4��?���nR�"X9�}�=���!����:����<��      �   9   x�. ��1	Дочернее общество №1
\.


��      �   �   x�]�Mj�0����)�.��/봍!��
R(�L���� ;)��@W9�|�3
l�;z�U��a�Ӂ��c��e�3[/������<K<Z�8���
+���-ӻ.s�����#����I�0���L�Td�!�Ԡ�y���w�En$��)��_�q�c���Y͒>��ߨ<)�!���OzQg�Jʵ���=}�m��H�7I�|[7S�[Te�v�uA���mV�:�\PU7�9eV�,�>������b�      �   �  x����N1��w�"�D2�����a Tj/P#Q�VJi/z�B�4���o���%!i�������e��i5j'���u{�R���4Q��r����0��M��Q{����sZ�uZ�gT�מ�2����<+2�Z0tǸʆ�j'�ؾ�e����>���7%�e�;J06�E�8K�8�����$�5��]p�����G��6W\pN�V�pJ:�ܸ6<��.R�k�I,�uZ�6��p��Sz��RܳFF��kXǨ@�D�ϲ	�5�g��n���
x�BtR�"M"j�����]G<Iw0�"��)L��%n�m��zKu��T��ȱr��dT��k?��}�0�)����+��A)-L�d�34�� "l������9��J=a�%�>�H�A)�P�H-*�XX%U`\g���us�7�9��t����h�)��rĲ���yʣ�p�b�xɬ�Qk=�I)���޾~��Y����˔.�l�j�����R�!B/:?P��v�����#|JG��n��)�T.�4�� ��K����<�s�f���>��^x��٣ghHb�טOy�~����~o0����Y-���A�H(*i�����U��"o�0Sa�T����t��D�6$+��ʷ��n�3n�W�VdKf���&D#��N��4$(䶣}W�8��e��nl`m�h6�g,��%���4.� Qy%���ّ�      �   C  x�U�[�� E���X��8�;�qܵɩꤿX䱏]v�^�ϼ����m�c{پ��u��6�c���׊��e��>Wvo1���܃�6�[0��2/��m�������880 ް/㯝��|���:�o#��V�g�07�`�`
��m�� �G_Ha�i6&o �ɳw�7�B_�;�M)��x��1j�Ý og�hE�1:�L����Z-m ��S�ҳ7�kĶ6SH�!R�=��ة0��0�i�����i�ro؀���5}���
���M��8-� �\@%	ے�Cs(8; ���۰�6�;G�����:�i��7��|��Ȁ">�Ol�/31]��b�/F���f�lH%��/�\�w䑸�J�=�Mz�
+;dSzڰL��ֱ)�N�Jw�e���v���yq:��,�s�~Ov$�bp��@�7ȏ�:�~jtj�q@T ���k5g�Dʕ�%V����o�ױ�8,��R�!šZs��9�tA�A��f���Ԏ�i��C,�Y�������줈ե�>�`IɬJ�C�(��H,F�׵�5h�zN*�3�&I�EPM4~4���A��ܔ*�ҧ���&�uŇ��ɐ�\������wyQ
�d$�N�j�M�����\E����A����;����4�M̚'r�@Z���+$�;)~��&�,�Y��5{��2��$�;��K|�m=3��:�b�C��.Z��N	��T�ƏV������2ifQ��&�4�C�C��~�T�U3�G�S)4���	��tZB�Ԥ��"�֑���Գȷ�p�]jS^�C$��hoQ~H33��s�fFH�������^%      �      x�URY��0���x�r���Orh�!E8hM~�b2�k��8�K���"�� ��R��2�^hs��c+�qi���i�IV�,��H*}3��|jZ ��f�%4��qT1~I�����!���4�bǅO���8>��F��'��w�ΜVp��dKujH�41I!g�{��Yش񰰌�&\$�lfW�x�Q�Pt�r/X��6ح�e0I屿8�����M&~�[�35e��U
��KYw�C�S�*�"�J��W�G3����	s+����q��OU�b�c�      �   �   x�uOI�0<O^�X�|�� =X��B"��������%U��=�L�m�E�����Q��,$�H��1KiJ�L�q��pZ�r�H�b�$�=Q�p/��O&��u�n8�vh���/��[�ɼ�3w�ͥܞW�s�޵��.�vҁ�$��yg�y���8      �   7   x�3�0�¾�/l��|���ދ�vrq^�{aPp�=P!c��-Pn� {E#o      �     x����J�@ ���U$���?��R��^v�U�h$m�ܪ"<y�P���V��0�Fn��=�ff�1B�3.����m�3ŕ ��t����0�i��kM �fh�p�Zi�dC�xƸ�N9d�J!�Mɞ�Z+�����iu��q�(%i�Z������ma/�����y�܇�5~��^��'y�M�Np�kFgP�Top���?��X��+\�o��vbFy�wř+;_��qx��1�6��7a=[ݼ.�`�_;M�����Gf4>��ޗ�	�i���Q����      �   T   x�Mʻ�0 ����8�dJ
V@� (BV8o��h�x�V�ع(���,�Y�Z�N���7;��B�?��AX9-�|lI�FU_�Z5�     