import React, { useState } from "react";
import { withRouter } from "react-router-dom";
import { useMutation } from "@apollo/client";
import { unauthClient } from "../../utils/GraphQLClient";
import SignUpForm from "../../components/organisms/Auth/SignUpForm";
import { SIGNUP } from "../../utils/GraphQLQueries/mutations";
import { useStore } from "../../store/hook-store";

export default withRouter((props) => {
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const [signup] = useMutation(SIGNUP, {
    client: unauthClient,
  });

  const showError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const signUp = async (data) => {
    setFormEnabled(true);

    if (signup) {
      await signup({
        variables: {
          criterias: data,
        },
      })
        .then((response) => {
          const { errors } = response;
          if (errors) {
            showError(
              "Đăng ký KHÔNG thành công",
              "Có lỗi xảy ra trong quá trình đăng ký"
            );
          } else {
            props.history.push("/auth/signin");
          }
        })
        .catch((error) => {
          setFormEnabled(true);
          showError(
            "Đăng ký KHÔNG thành công",
            "Có lỗi xảy ra trong quá trình đăng ký"
          );
        });
    }
  };

  return (
    <SignUpForm
      signUp={(data) => signUp(data)}
      showValidationError={showError}
      isFormEnabled={isFormEnabled}
    />
  );
});
