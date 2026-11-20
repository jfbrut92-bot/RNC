<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:otc="otc">
  <xsl:output method="xml" indent="yes" encoding="utf-8"/>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="otc:listPcontrole/otc:item">
      <xsl:element name="itemPControle" namespace="otc">
        <xsl:apply-templates/>
      </xsl:element>
  </xsl:template>

  <xsl:template match="otc:pDocConstate/otc:item">
      <xsl:element name="itemPDocConstate" namespace="otc">
        <xsl:apply-templates/>
      </xsl:element>
  </xsl:template>

  <xsl:template match="otc:RLocalisation/otc:item">
      <xsl:element name="itemRLocalisation" namespace="otc">
        <xsl:apply-templates/>
      </xsl:element>
  </xsl:template>

  <xsl:template match="otc:rAno/otc:item">
      <xsl:element name="itemRAno" namespace="otc">
        <xsl:apply-templates/>
      </xsl:element>
  </xsl:template>

  <xsl:template match="*[@xsi:nil = 'true']" />

</xsl:stylesheet>
