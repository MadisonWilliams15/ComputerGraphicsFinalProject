// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: mlagents/envs/communicator_objects/unity_rl_output.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace MLAgents.CommunicatorObjects {

  /// <summary>Holder for reflection information generated from mlagents/envs/communicator_objects/unity_rl_output.proto</summary>
  public static partial class UnityRlOutputReflection {

    #region Descriptor
    /// <summary>File descriptor for mlagents/envs/communicator_objects/unity_rl_output.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static UnityRlOutputReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjhtbGFnZW50cy9lbnZzL2NvbW11bmljYXRvcl9vYmplY3RzL3VuaXR5X3Js",
            "X291dHB1dC5wcm90bxIUY29tbXVuaWNhdG9yX29iamVjdHMaM21sYWdlbnRz",
            "L2VudnMvY29tbXVuaWNhdG9yX29iamVjdHMvYWdlbnRfaW5mby5wcm90byKj",
            "AgoSVW5pdHlSTE91dHB1dFByb3RvEkwKCmFnZW50SW5mb3MYAiADKAsyOC5j",
            "b21tdW5pY2F0b3Jfb2JqZWN0cy5Vbml0eVJMT3V0cHV0UHJvdG8uQWdlbnRJ",
            "bmZvc0VudHJ5GkkKEkxpc3RBZ2VudEluZm9Qcm90bxIzCgV2YWx1ZRgBIAMo",
            "CzIkLmNvbW11bmljYXRvcl9vYmplY3RzLkFnZW50SW5mb1Byb3RvGm4KD0Fn",
            "ZW50SW5mb3NFbnRyeRILCgNrZXkYASABKAkSSgoFdmFsdWUYAiABKAsyOy5j",
            "b21tdW5pY2F0b3Jfb2JqZWN0cy5Vbml0eVJMT3V0cHV0UHJvdG8uTGlzdEFn",
            "ZW50SW5mb1Byb3RvOgI4AUoECAEQAkIfqgIcTUxBZ2VudHMuQ29tbXVuaWNh",
            "dG9yT2JqZWN0c2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::MLAgents.CommunicatorObjects.AgentInfoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::MLAgents.CommunicatorObjects.UnityRLOutputProto), global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Parser, new[]{ "AgentInfos" }, null, null, new pbr::GeneratedClrTypeInfo[] { new pbr::GeneratedClrTypeInfo(typeof(global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Types.ListAgentInfoProto), global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Types.ListAgentInfoProto.Parser, new[]{ "Value" }, null, null, null),
            null, })
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class UnityRLOutputProto : pb::IMessage<UnityRLOutputProto> {
    private static readonly pb::MessageParser<UnityRLOutputProto> _parser = new pb::MessageParser<UnityRLOutputProto>(() => new UnityRLOutputProto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UnityRLOutputProto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::MLAgents.CommunicatorObjects.UnityRlOutputReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UnityRLOutputProto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UnityRLOutputProto(UnityRLOutputProto other) : this() {
      agentInfos_ = other.agentInfos_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UnityRLOutputProto Clone() {
      return new UnityRLOutputProto(this);
    }

    /// <summary>Field number for the "agentInfos" field.</summary>
    public const int AgentInfosFieldNumber = 2;
    private static readonly pbc::MapField<string, global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Types.ListAgentInfoProto>.Codec _map_agentInfos_codec
        = new pbc::MapField<string, global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Types.ListAgentInfoProto>.Codec(pb::FieldCodec.ForString(10), pb::FieldCodec.ForMessage(18, global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Types.ListAgentInfoProto.Parser), 18);
    private readonly pbc::MapField<string, global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Types.ListAgentInfoProto> agentInfos_ = new pbc::MapField<string, global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Types.ListAgentInfoProto>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<string, global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Types.ListAgentInfoProto> AgentInfos {
      get { return agentInfos_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UnityRLOutputProto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UnityRLOutputProto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!AgentInfos.Equals(other.AgentInfos)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= AgentInfos.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      agentInfos_.WriteTo(output, _map_agentInfos_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += agentInfos_.CalculateSize(_map_agentInfos_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UnityRLOutputProto other) {
      if (other == null) {
        return;
      }
      agentInfos_.Add(other.agentInfos_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 18: {
            agentInfos_.AddEntriesFrom(input, _map_agentInfos_codec);
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the UnityRLOutputProto message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public sealed partial class ListAgentInfoProto : pb::IMessage<ListAgentInfoProto> {
        private static readonly pb::MessageParser<ListAgentInfoProto> _parser = new pb::MessageParser<ListAgentInfoProto>(() => new ListAgentInfoProto());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pb::MessageParser<ListAgentInfoProto> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pbr::MessageDescriptor Descriptor {
          get { return global::MLAgents.CommunicatorObjects.UnityRLOutputProto.Descriptor.NestedTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        pbr::MessageDescriptor pb::IMessage.Descriptor {
          get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public ListAgentInfoProto() {
          OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public ListAgentInfoProto(ListAgentInfoProto other) : this() {
          value_ = other.value_.Clone();
          _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public ListAgentInfoProto Clone() {
          return new ListAgentInfoProto(this);
        }

        /// <summary>Field number for the "value" field.</summary>
        public const int ValueFieldNumber = 1;
        private static readonly pb::FieldCodec<global::MLAgents.CommunicatorObjects.AgentInfoProto> _repeated_value_codec
            = pb::FieldCodec.ForMessage(10, global::MLAgents.CommunicatorObjects.AgentInfoProto.Parser);
        private readonly pbc::RepeatedField<global::MLAgents.CommunicatorObjects.AgentInfoProto> value_ = new pbc::RepeatedField<global::MLAgents.CommunicatorObjects.AgentInfoProto>();
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public pbc::RepeatedField<global::MLAgents.CommunicatorObjects.AgentInfoProto> Value {
          get { return value_; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override bool Equals(object other) {
          return Equals(other as ListAgentInfoProto);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public bool Equals(ListAgentInfoProto other) {
          if (ReferenceEquals(other, null)) {
            return false;
          }
          if (ReferenceEquals(other, this)) {
            return true;
          }
          if(!value_.Equals(other.value_)) return false;
          return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override int GetHashCode() {
          int hash = 1;
          hash ^= value_.GetHashCode();
          if (_unknownFields != null) {
            hash ^= _unknownFields.GetHashCode();
          }
          return hash;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override string ToString() {
          return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void WriteTo(pb::CodedOutputStream output) {
          value_.WriteTo(output, _repeated_value_codec);
          if (_unknownFields != null) {
            _unknownFields.WriteTo(output);
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public int CalculateSize() {
          int size = 0;
          size += value_.CalculateSize(_repeated_value_codec);
          if (_unknownFields != null) {
            size += _unknownFields.CalculateSize();
          }
          return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(ListAgentInfoProto other) {
          if (other == null) {
            return;
          }
          value_.Add(other.value_);
          _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(pb::CodedInputStream input) {
          uint tag;
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                break;
              case 10: {
                value_.AddEntriesFrom(input, _repeated_value_codec);
                break;
              }
            }
          }
        }

      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
