using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedTextMaker.Model
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "fixedText",  Namespace = "", IsNullable = false)]
    public partial class FixedTextDefines
    {

        private List<FixedTextRecord> recordField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("record")]
        public List<FixedTextRecord> Records
        {
            get
            {
                return this.recordField;
            }
            set
            {
                this.recordField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FixedTextRecord
    {

        private List<FixedTextRecordItems> itemField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("item")]
        public List<FixedTextRecordItems> Items
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FixedTextRecordItems
    {

        private string nameField;

        private int lengthField;

        private string padcharField;

        private string fixedcharField;

        private bool recognitionField;

        private bool recognitionFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("length")]
        public int Length
        {
            get
            {
                return this.lengthField;
            }
            set
            {
                this.lengthField = value;
            }
        }

        /// <summary>
        /// パディングする文字列を取得または設定します。
        /// パディングは最初の一文字のみ有効です。
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute("padchar")]
        public string Padchar
        {
            get
            {
                return this.padcharField;
            }
            set
            {
                this.padcharField = value;
            }
        }

        /// <summary>
        /// テキストボックスに入る固定文字を取得または設定します。
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute("fixedtext")]
        public string FixedText
        {
            get
            {
                return this.fixedcharField;
            }
            set
            {
                this.fixedcharField = value;
            }
        }

        /// <summary>
        /// レコードを識別するキーか否かを取得または設定します。
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute("recognition")]
        public bool Recognition
        {
            get
            {
                return this.recognitionField;
            }
            set
            {
                this.recognitionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RecognitionSpecified
        {
            get
            {
                return this.recognitionFieldSpecified;
            }
            set
            {
                this.recognitionFieldSpecified = value;
            }
        }
    }
}
