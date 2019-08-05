import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary } from "../../../components/atoms/Buttons/Buttons";
import LabelAndTextbox from "../../molecules/InfoWithLabels/LabelAndTextbox";
import ProfileUpdateModel from "../../../models/ProfileUpdateModel";
import { checkValidity } from "../../../utils/Validity";
import { PanelFooter } from "../../../components/atoms/Panels";

const MainPanel = styled(PanelBody)`
  border-radius: ${p => p.theme.borderRadius.normal};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  margin-bottom: ${p => p.theme.size.normal};
  background-color: ${p => p.theme.color.white};
`;

const Root = styled.div`
  position: relative;
  border-radius: ${p => p.theme.borderRadius.normal};
`;

const FormGroup = styled.div`
  margin-bottom: ${p => p.theme.size.exTiny};
`;

const SubmitButton = styled(ButtonPrimary)`
  font-size: ${p => p.theme.fontSize.small};
  cursor: pointer;
  border: 1px solid ${p => p.theme.color.primaryLight};

  :hover {
    color: ${p => p.theme.color.light};
  }

  :disabled {
    background-color: ${p => p.theme.color.primaryLight};
    color: ${p => p.theme.color.neutral};
    cursor: auto;
  }
`;

const FormFooter = styled(PanelFooter)`
  padding-left: 0;
  padding-right: 0;
`;

export default class extends Component {
  constructor(props) {
    super(props);

    this._isMounted = false;
    const { userInfo } = props;
    const { displayName, firstname, lastname } = userInfo;

    this.formData = ProfileUpdateModel;
    this.formData.displayName.value = displayName;
    this.formData.displayName.isValid = !!displayName;
    this.formData.firstname.value = firstname;
    this.formData.firstname.isValid = !!firstname;
    this.formData.lastname.value = lastname;
    this.formData.lastname.isValid = !!lastname;
    this.state = {
      shouldRender: false
    };
  }

  // #region Life Cycle
  componentDidMount() {
    this._isMounted = true;
  }

  componentWillUnmount() {
    this._isMounted = false;
  }
  // #endregion Life Cycle

  onTextboxChange = e => {
    this.formData = this.formData || {};
    const { name, value } = e.target;

    // Validate when input
    this.formData[name].isValid = checkValidity(this.formData, value, name);
    this.formData[name].value = value;

    if (!!this._isMounted) {
      this.setState({
        shouldRender: true
      });
    }
  };

  checkIsFormValid = () => {
    let isFormValid = false;
    for (let formIdentifier in this.formData) {
      isFormValid = this.formData[formIdentifier].isValid;
      if (!isFormValid) {
        break;
      }
    }

    return isFormValid;
  };

  onUpdate = e => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in this.formData) {
      isFormValid = this.formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        this.props.showValidationError(
          "Thông tin bạn nhập có thể bị sai",
          "Có thể bạn nhập sai thông tin này, vui lòng kiểm tra và nhập lại"
        );
      }
    }

    if (!!isFormValid) {
      const profileData = {};
      for (const formIdentifier in this.formData) {
        profileData[formIdentifier] = this.formData[formIdentifier].value;
      }

      this.props.onUpdate(profileData);
    }
  };

  render() {
    const { userInfo } = this.props;
    const { displayName, firstname, lastname } = this.formData;
    const isFormValid = this.checkIsFormValid();

    return (
      <MainPanel>
        <form onSubmit={e => this.onUpdate(e)} method="POST">
          <Root>
            <h4>Cập nhật thông tin cá nhân</h4>
            {userInfo ? (
              <Fragment>
                <FormGroup>
                  <LabelAndTextbox
                    label="Tên hiển thị"
                    name="displayName"
                    value={displayName.value}
                    onChange={this.onTextboxChange}
                  />
                </FormGroup>
                <FormGroup>
                  <LabelAndTextbox
                    label="Họ"
                    name="lastname"
                    value={lastname.value}
                    onChange={this.onTextboxChange}
                  />
                </FormGroup>
                <FormGroup>
                  <LabelAndTextbox
                    label="Tên"
                    name="firstname"
                    value={firstname.value}
                    onChange={this.onTextboxChange}
                  />
                </FormGroup>
              </Fragment>
            ) : null}
          </Root>
          <FormFooter>
            <SubmitButton
              type="submit"
              disabled={!this.props.isFormEnabled || !isFormValid}
            >
              Cập Nhật
            </SubmitButton>
          </FormFooter>
        </form>
      </MainPanel>
    );
  }
}
