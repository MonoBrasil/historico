<?xml version="1.0" encoding="ISO-8859-15"?>
<xsl:stylesheet
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	version="1.0"
	xmlns:UML='org.omg.xmi.namespace.UML'
	xmlns:xmi="http://www.omg.org/XMI"
	xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	xmlns:uml="http://schema.omg.org/spec/uml/2.0/uml.xmi"
>
	<xsl:namespace-alias stylesheet-prefix="uml" result-prefix="uml"/>
	<xsl:output method="xml" encoding="ISO-8859-15" indent="yes" />

	<xsl:template match="XMI[@xmi.version='1.0']">
		<xmi:XMI xmi:version="2.0">
			<xsl:apply-templates select="XMI.content/*" />
		</xmi:XMI>
	</xsl:template>
	
	<xsl:template match="Model_Management.Model">
		<xsl:element name="uml:Model">
			<xsl:apply-templates select="@*|*"/>
		</xsl:element>
	</xsl:template>
	
	<xsl:template match="Foundation.Core.Class.isActive">
		<xsl:attribute name="isActive"><xsl:value-of select="@xmi.value"/><xsl:value-of select="."/></xsl:attribute>
	</xsl:template>
	<xsl:template match="Foundation.Core.Feature.ownerScope[not(@xmi.value='instance')]">
		<xsl:attribute name="isStatic">true</xsl:attribute>
	</xsl:template>
	<xsl:template match="Foundation.Core.GeneralizableElement.generalization">
		<generalization xmi:id="{current()/Foundation.Core.Generalization/@xmi.idref}" general="{//Foundation.Core.Generalization[@xmi.id=current()/Foundation.Core.Generalization/@xmi.idref]/Foundation.Core.Generalization.parent/Foundation.Core.GeneralizableElement/@xmi.idref}" />
	</xsl:template>
	<xsl:template match="Foundation.Core.GeneralizableElement.isAbstract">
		<xsl:attribute name="isAbstract"><xsl:value-of select="@xmi.value"/><xsl:value-of select="."/></xsl:attribute>
	</xsl:template>
	<xsl:template match="Foundation.Core.GeneralizableElement.isRoot">
		<xsl:attribute name="isRoot"><xsl:value-of select="@xmi.value"/><xsl:value-of select="."/></xsl:attribute>
	</xsl:template>
	<xsl:template match="Foundation.Core.GeneralizableElement.isLeaf">
		<xsl:attribute name="isLeaf"><xsl:value-of select="@xmi.value"/><xsl:value-of select="."/></xsl:attribute>
	</xsl:template>

	<!-- interface realizations are treated as Abstrations with the "realization" stereotype -->
	<xsl:template match="Foundation.Core.ModelElement.clientDependency">
		<xsl:variable name="abstraction" select="//Foundation.Core.Abstraction[@xmi.id=current()/Foundation.Core.Dependency/@xmi.idref]" />
		<xsl:if test="//Foundation.Extension_Mechanisms.Stereotype[Foundation.Core.ModelElement.name='realize']/Foundation.Extension_Mechanisms.Stereotype.extendedElement/Foundation.Core.ModelElement[@xmi.idref=$abstraction/@xmi.id]">
			<interfaceRealization xmi:id="{current()/Foundation.Core.Dependency/@xmi.idref}" contract="{$abstraction/Foundation.Core.Dependency.supplier/Foundation.Core.ModelElement/@xmi.idref}"/>
		</xsl:if> 
	</xsl:template>

	<xsl:template match="Foundation.Core.ModelElement.name">
		<xsl:attribute name="name"><xsl:value-of select="@xmi.value"/><xsl:value-of select="."/></xsl:attribute>
	</xsl:template>
	<xsl:template match="Foundation.Core.ModelElement.visibility">
		<xsl:attribute name="visibility"><xsl:value-of select="@xmi.value"/><xsl:value-of select="."/></xsl:attribute>
	</xsl:template>

	<xsl:template match="Foundation.Core.Namespace.ownedElement">
		<xsl:for-each select="*">
			<xsl:choose>
				<xsl:when test="name(.)='UML:Package'">
					<nestedPackage>
						<xsl:apply-templates select="@*|*" />
					</nestedPackage>
				</xsl:when>
				<xsl:when test="name(.)='Foundation.Core.Class'">
					<ownedType xsi:type="uml:Class">
						<xsl:apply-templates select="@*|*" />
					</ownedType>
				</xsl:when>
				<xsl:when test="name(.)='Foundation.Core.DataType'">
					<ownedType xsi:type="uml:DataType">
						<xsl:apply-templates select="@*|*" />
					</ownedType>
				</xsl:when>
				<xsl:when test="name(.)='Foundation.Core.Interface'">
					<ownedType xsi:type="uml:Interface">
						<xsl:apply-templates select="@*|*" />
					</ownedType>
				</xsl:when>
				<xsl:otherwise>
					<xsl:comment>WARNING: Cannot handle <xsl:value-of select="local-name(.)"/> elements.</xsl:comment>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="Foundation.Core.Classifier.feature">
		<xsl:for-each select="*">
			<xsl:if test="name(.)='Foundation.Core.Attribute'">
				<ownedAttribute xmi:id="{@xmi.id}">
					<xsl:if test="Foundation.Core.StructuralFeature.type">
						<xsl:attribute name="type">
							<xsl:value-of select="Foundation.Core.StructuralFeature.type/*/@xmi.idref"/> 
						</xsl:attribute>
					</xsl:if>
					<xsl:apply-templates select="@*|*" />
				</ownedAttribute>
			</xsl:if>
			<xsl:if test="name(.)='Foundation.Core.Operation'">
				<ownedOperation>
					<xsl:apply-templates select="@*|*" />
				</ownedOperation>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="Foundation.Core.Parameter.kind">
		<xsl:attribute name="direction"><xsl:value-of select="@xmi.value"/></xsl:attribute>
	</xsl:template>
	
	<xsl:template match="Foundation.Core.StructuralFeature.multiplicity">
		<xsl:apply-templates select="Foundation.Data_Types.Multiplicity/Foundation.Data_Types.Multiplicity.range/Foundation.Data_Types.MultiplicityRange[1]/Foundation.Data_Types.MultiplicityRange.lower"/>
		<xsl:apply-templates select="Foundation.Data_Types.Multiplicity/Foundation.Data_Types.Multiplicity.range/Foundation.Data_Types.MultiplicityRange[last()]/Foundation.Data_Types.MultiplicityRange.upper"/>
	</xsl:template>
	
	<xsl:template match="Foundation.Data_Types.MultiplicityRange.lower">
		<xsl:attribute name="lower"><xsl:value-of select="."/></xsl:attribute>
	</xsl:template>
	<xsl:template match="Foundation.Data_Types.MultiplicityRange.upper">
		<xsl:attribute name="upper">
			<xsl:choose>
				<xsl:when test=".='-1'">*</xsl:when>
				<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>


	<xsl:template match="Foundation.Core.BehavioralFeature.parameter">
		<xsl:for-each select="Foundation.Core.Parameter">
			<ownedParameter>
				<xsl:apply-templates select="@*" />
				<xsl:if test="Foundation.Core.Parameter.type">
					<xsl:attribute name="type">
						<xsl:value-of select="Foundation.Core.Parameter.type/*/@xmi.idref"/> 
					</xsl:attribute>
				</xsl:if>
				<xsl:apply-templates select="*" />
			</ownedParameter>
		</xsl:for-each>
	</xsl:template>


	<xsl:template match="UML:Diagram">
		<xsl:comment>TODO: convert diagram &quot;<xsl:value-of select="@name"/>&quot;</xsl:comment>
	</xsl:template>

	<xsl:template match="UML:Model">
		<xsl:apply-templates select="UML:Namespace.ownedElement/*" />
	</xsl:template>

	<xsl:template match="UML:Association">
		<uml:Association>
			<xsl:for-each select="UML:Association.connection/UML:AssociationEnd">
				<ownedEnd type="{UML:AssociationEnd.participant/*[1]/@xmi.idref}">
					<xsl:apply-templates select="@*" />
				</ownedEnd>
			</xsl:for-each>
		</uml:Association>
	</xsl:template>
	
	<xsl:template match="UML:UseCase.extend">
		<xsl:variable name="ref" select="UML:Extend/@xmi.idref"/>
		<extend extendedCase="{//UML:Extend[@xmi.id=$ref]/UML:Extend.base/UML:UseCase/@xmi.idref}"/>
	</xsl:template>
	
	<xsl:template match="UML:UseCase.include">
		<xsl:variable name="ref" select="UML:Include/@xmi.idref"/>
		<include addition="{//UML:Include[@xmi.id=$ref]/UML:Include.addition/UML:UseCase/@xmi.idref}"/>
	</xsl:template>

	<xsl:template match="@*"><xsl:copy /></xsl:template>
	
	<xsl:template match="@xmi.id">
		<xsl:attribute name="xmi:id"><xsl:value-of select="."/></xsl:attribute>
	</xsl:template>

	<!-- elements that must not appear at the output -->
	<xsl:template match="UML:Extend|UML:Include|Foundation.Core.ModelElement.taggedValue|UML:TagDefinition|UML:Parameter.type|UML:Stereotype[@name='realize']|UML:StructuralFeature.type"/>
	<xsl:template match="@isNavigable"/>
</xsl:stylesheet>
