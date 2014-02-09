<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="c2684573-4a8c-4361-b8c8-aaa072334f16" namespace="ELMAH_Viewer.Configuration" xmlSchemaNamespace="urn:ELMAH_Viewer.Configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="SettingsSection" codeGenOptions="XmlnsProperty" xmlSectionName="settings">
      <elementProperties>
        <elementProperty name="SavedConnections" isRequired="false" isKey="false" isDefaultCollection="true" xmlName="savedConnections" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/c2684573-4a8c-4361-b8c8-aaa072334f16/SavedConnectionsCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Sources" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="sources" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/c2684573-4a8c-4361-b8c8-aaa072334f16/SourcesElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="SavedConnectionsCollection" xmlItemName="connection" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods, ICollection">
      <itemType>
        <configurationElementMoniker name="/c2684573-4a8c-4361-b8c8-aaa072334f16/ConnectionElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ConnectionElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/c2684573-4a8c-4361-b8c8-aaa072334f16/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="SourcesElement">
      <attributeProperties>
        <attributeProperty name="Location" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="location" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/c2684573-4a8c-4361-b8c8-aaa072334f16/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>