<?xml version="1.0" encoding="ISO-8859-15"?>
<xsl:stylesheet
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	version="1.0"
	xmlns:di="org.omg.xmi.namespace.UML"
	xmlns:UML='org.omg.xmi.namespace.UML'
	xmlns:xmi="http://www.omg.org/XMI"
	xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	xmlns:uml="http://schema.omg.org/spec/uml/2.0/uml.xmi"
	xmlns:uml2bridge="http://monouml.sourceforge.net/uml2bridge"
>
	<xsl:namespace-alias stylesheet-prefix="uml" result-prefix="uml"/>
	<xsl:output method="xml" encoding="ISO-8859-15" indent="yes" />

	<xsl:template match="XMI[@xmi.version='1.2']">
		<xmi:XMI xmi:version="2.0">
			<xsl:apply-templates select="XMI.content/*" />
		</xmi:XMI>
	</xsl:template>

<!--
	<xsl:template match="UML:*">
		<xsl:element name="uml:{local-name(.)}">
			<xsl:apply-templates select="@*|*"/>
		</xsl:element>
	</xsl:template>
-->

	<xsl:template match="UML:Association.connection">
		<xsl:for-each select="UML:AssociationEnd">
			<xsl:choose>
				<xsl:when test="@isNavigable='false' or not(UML:AssociationEnd.participant/UML:Class | UML:AssociationEnd.participant/UML:DataType | UML:AssociationEnd.participant/UML:Interface)">
					<ownedEnd xmi:id="{@xmi.id}"><xsl:attribute name="type"><xsl:value-of select="UML:AssociationEnd.participant/*[1]/@xmi.idref" /></xsl:attribute>
					<xsl:apply-templates select="UML:AssociationEnd.multiplicity/*" />
					<xsl:apply-templates select="@*|*" />
					</ownedEnd>
				</xsl:when>
				<xsl:otherwise>
					<memberEnd xmi:idref="{@xmi.id}" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="UML:BehavioralFeature.parameter">
		<xsl:for-each select="UML:Parameter">
			<ownedParameter>
				<xsl:apply-templates select="@*[not(name(.)='kind')]" />
				<xsl:if test="@kind">
					<xsl:attribute name="direction"><xsl:value-of select="@kind"/></xsl:attribute>
				</xsl:if>
				<xsl:if test="UML:Parameter.type">
					<xsl:attribute name="type">
						<xsl:value-of select="UML:Parameter.type/*/@xmi.idref"/> 
					</xsl:attribute>
				</xsl:if>
				<xsl:apply-templates select="*" />
			</ownedParameter>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="UML:Multiplicity">
		<xsl:attribute name="lower"><xsl:value-of select="UML:Multiplicity.range[1]/UML:MultiplicityRange[1]/@lower" /></xsl:attribute>
		<xsl:variable name="u" select="UML:Multiplicity.range[1]/UML:MultiplicityRange[1]/@upper" />
		<xsl:attribute name="upper"><xsl:choose><xsl:when test="$u='-1'">*</xsl:when><xsl:otherwise><xsl:value-of select="$u" /></xsl:otherwise></xsl:choose></xsl:attribute>
	</xsl:template>

	<xsl:template match="UML:Namespace.ownedElement">
		<xsl:for-each select="UML:*">
			<xsl:choose>
				<xsl:when test="name(.)='UML:Package'">
					<nestedPackage>
						<xsl:apply-templates select="@*|*" />
					</nestedPackage>
				</xsl:when>
				<xsl:when test="name(.)='UML:Actor' or name(.)='UML:Association' or name(.)='UML:UseCase'">
					<ownedType xsi:type="uml:{local-name(.)}">
						<xsl:apply-templates select="@*|*" />
					</ownedType>
				</xsl:when>
				<xsl:when test="name(.)='UML:Class' or name(.)='UML:DataType' or name(.)='UML:Interface'">
					<xsl:variable name="id" select="@xmi.id" />
					<ownedType xsi:type="uml:{local-name(.)}">
						<xsl:apply-templates select="@*|*" />
						<!-- Looks for owned ends of associations -->
						<xsl:for-each select="//UML:AssociationEnd[@isNavigable='true']">
							<!--
							the idea is to find navigable association ends, 
							where the participant of the other end is the current class.
							-->
							<xsl:variable name="assocEndId" select="@xmi.id" />
							<xsl:if test="../UML:AssociationEnd[@xmi.id!=$assocEndId]/UML:AssociationEnd.participant/*[1]/@xmi.idref=$id">
								<ownedAttribute xmi:id="{@xmi.id}">
									<xsl:attribute name="association"><xsl:value-of select="ancestor::UML:Association/@xmi.id" /></xsl:attribute>
									<xsl:attribute name="type"><xsl:value-of select="UML:AssociationEnd.participant/*[1]/@xmi.idref" /></xsl:attribute>
									<xsl:apply-templates select="@*|*" />
								</ownedAttribute>
							</xsl:if>
						</xsl:for-each>
					</ownedType>
				</xsl:when>
				<xsl:otherwise>
					<xsl:comment>WARNING: Cannot handle <xsl:value-of select="local-name(.)"/> elements.</xsl:comment>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="UML:Classifier.feature">
		<xsl:for-each select="UML:*">
			<xsl:if test="name(.)='UML:Attribute'">
				<ownedAttribute>
					<xsl:if test="UML:StructuralFeature.type">
						<xsl:attribute name="type">
							<xsl:value-of select="UML:StructuralFeature.type/*/@xmi.idref"/> 
						</xsl:attribute>
					</xsl:if>
					<xsl:apply-templates select="@*|*" />
				</ownedAttribute>
			</xsl:if>
			<xsl:if test="name(.)='UML:Operation'">
				<ownedOperation>
					<xsl:if test="UML:StructuralFeature.type">
						<xsl:attribute name="type">
							<xsl:value-of select="UML:StructuralFeature.type/*/@xmi.idref"/> 
						</xsl:attribute>
					</xsl:if>
					<xsl:apply-templates select="@*|*" />
				</ownedOperation>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="UML:GeneralizableElement.generalization">
		<xsl:variable name="generalizationid" select="UML:Generalization/@xmi.idref" />
		<generalization xmi:id="{$generalizationid}" general="{//UML:Generalization[@xmi.id=$generalizationid]/UML:Generalization.parent/*[1]/@xmi.idref}" />
	</xsl:template>
	
	<xsl:template match="UML:ModelElement.clientDependency">
		<xsl:if test="UML:Abstraction">
			<!-- Poseidon treates interface implementations as Abstrations with the "realization" stereotype -->
			<xsl:variable name="realizeStereotypeID" select="//UML:Stereotype[@name='realize']/@xmi.id" />
			<xsl:variable name="abstractionID" select="UML:Abstraction/@xmi.idref" />
			<xsl:variable name="abstraction" select="//UML:Abstraction[@xmi.id=$abstractionID]" />
			<xsl:if test="$abstraction/UML:ModelElement.stereotype/UML:Stereotype[@xmi.idref=$realizeStereotypeID]">
				<interfaceRealization xmi:id="{$abstractionID}" contract="{$abstraction/UML:Dependency.supplier/*[1]/@xmi.idref}"/>
			</xsl:if> 
		</xsl:if> 
	</xsl:template>

	<xsl:template match="UML:Model">
		<xsl:element name="uml:Model">
			<xsl:apply-templates select="@*|*"/>
		</xsl:element>
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
		<extend
			extendedCase="{//UML:Extend[@xmi.id=$ref]/UML:Extend.base/UML:UseCase/@xmi.idref}"
			xmi:id="{$ref}"
		/>
	</xsl:template>
	
	<xsl:template match="UML:UseCase.include">
		<xsl:variable name="ref" select="UML:Include/@xmi.idref"/>
		<include
			addition="{//UML:Include[@xmi.id=$ref]/UML:Include.addition/UML:UseCase/@xmi.idref}"
			xmi:id="{$ref}"
		/>
	</xsl:template>

	<xsl:template match="@*"><xsl:copy /></xsl:template>
	
	<xsl:template match="@xmi.id">
		<xsl:attribute name="xmi:id"><xsl:value-of select="."/></xsl:attribute>
	</xsl:template>

	<!-- elements that must not appear at the output -->
	<xsl:template match="UML:Extend|UML:Include|UML:ModelElement.taggedValue|UML:TagDefinition|UML:Parameter.type|UML:Stereotype[@name='realize']|UML:StructuralFeature.type"/>
	<xsl:template match="@isNavigable"/>
	
	<!-- diagramming stuff -->
	<xsl:template match="UML:Diagram">
		<di:Diagram>
			<xsl:apply-templates select="@*|*"/>
		</di:Diagram>
	</xsl:template>

	<xsl:template match="UML:Diagram.owner">
		<namespace><xsl:apply-templates select="@*|*" /></namespace>
	</xsl:template>
	
	<xsl:template match="UML:GraphConnector.graphEdge">
		<xsl:for-each select ="UML:GraphEdge">
			<graphEdge xmi:idref="{@xmi.idref}" />
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="UML:GraphEdge.anchor">
		<xsl:for-each select ="UML:GraphConnector">
			<anchor xmi:idref="{@xmi.idref}" />
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="UML:GraphEdge.waypoints">
		<xsl:for-each select ="XMI.field[not(XMI.field[1]='0.0' and XMI.field[2]='0.0')]">
			<waypoints x="{XMI.field[1]}" y="{XMI.field[2]}" />
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="UML:GraphElement.position|UML:GraphConnector.position">
		<position x="{XMI.field[1]}" y="{XMI.field[2]}" />
	</xsl:template>

	<xsl:template match="UML:GraphNode.size">
		<size height="{XMI.field[2]}" width="{XMI.field[1]}" />
	</xsl:template>

	<xsl:template match="UML:Diagram.viewport">
		<viewport x="{XMI.field[1]}" y="{XMI.field[2]}" />
	</xsl:template>
	
	<xsl:template match="UML:GraphElement.anchorage">
		<xsl:for-each select="UML:GraphConnector">
			<anchorage xmi:id="{@xmi.id}"><xsl:apply-templates select="@*|*" /></anchorage>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="UML:GraphElement.contained">
		<xsl:for-each select="UML:GraphNode">
			<contained xsi:type="di:GraphNode">
				<xsl:apply-templates select="@*|*" />
			</contained>
		</xsl:for-each>
		<xsl:for-each select="UML:GraphEdge">
			<contained xsi:type="di:GraphEdge">
				<xsl:apply-templates select="@*|*" />
			</contained>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="UML:GraphElement.semanticModel">
		<semanticModel><xsl:apply-templates select="@*|*" /></semanticModel>
	</xsl:template>
	
	<xsl:template match="UML:SimpleSemanticModelElement">
		<xsl:attribute name="xsi:type">di:SimpleSemanticModelElement</xsl:attribute>
		<xsl:apply-templates select="@*"/>
	</xsl:template>
	
	<xsl:template match="UML:Uml1SemanticModelBridge">
		<xsl:attribute name="xsi:type">uml2bridge:Uml2SemanticModelBridge</xsl:attribute>
		<xsl:attribute name="element"><xsl:value-of select="UML:Uml1SemanticModelBridge.element/*[1]/@xmi.idref"/></xsl:attribute>
		<xsl:apply-templates select="@*"/>
	</xsl:template>

	<!-- elements that must not appear at the output -->
</xsl:stylesheet>
