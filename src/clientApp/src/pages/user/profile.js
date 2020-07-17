import React, { useState, useEffect, useContext } from "react";
import { withRouter } from "react-router-dom";
import Profile from "../../components/organisms/User/Profile";
import { SessionContext } from "../../store/context/SessionContext";
import { GET_USER_INFO } from "../../utils/GraphQLQueries/queries";
import { useQuery } from "@apollo/client";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import Loading from "../../components/atoms/Loading";
import { useStore } from "../../store/hook-store";
import { parseUserInfo } from "../../services/UserService";

export default withRouter((props) => {
  const [isEditCoverMode, setEditCoverMode] = useState(false);
  const _baseUrl = "/profile";
  const sessionContext = useContext(SessionContext);
  const { match } = props;
  const { params } = match;
  const { userId, pageNumber } = params;
  const { loading, error, data, refetch } = useQuery(GET_USER_INFO, {
    variables: {
      criterias: {
        userId,
      },
    },
  });

  const pages = [
    {
      path: [`${_baseUrl}/:userId/about`],
      dir: "user-about",
    },
    {
      path: [`${_baseUrl}/:userId/update`],
      dir: "user-update",
    },
    {
      path: [`${_baseUrl}/:userId/security`],
      dir: "user-security",
    },
    {
      path: [
        `${_baseUrl}/:userId/posts`,
        `${_baseUrl}/:userId/posts/page/:pageNumber`,
      ],
      dir: "user-posts",
    },
    {
      path: [
        `${_baseUrl}/:userId/products`,
        `${_baseUrl}/:userId/products/page/:pageNumber`,
      ],
      dir: "user-products",
    },
    {
      path: [
        `${_baseUrl}/:userId/farms`,
        `${_baseUrl}/:userId/farms/page/:pageNumber`,
      ],
      dir: "user-farms",
    },
    {
      path: [
        `${_baseUrl}/:userId/followings`,
        `${_baseUrl}/:userId/followings/page/:pageNumber`,
      ],
      dir: "user-followings",
    },
    {
      path: [
        `${_baseUrl}/:userId`,
        `${_baseUrl}/:userId/feeds`,
        `${_baseUrl}/:userId/feeds/page/:pageNumber`,
      ],
      dir: "user-feeds",
    },
  ];

  const [state, dispatch] = useStore(false);
  useEffect(() => {
    if (state.type === "AVATAR_UPDATED") {
      refetch();
    }
  }, [state, refetch, sessionContext]);

  if (loading) {
    return <Loading>Loading</Loading>;
  }

  if (error) {
    return <ErrorBlock>Error</ErrorBlock>;
  }

  if (!data) {
    return <ErrorBlock>Not Found</ErrorBlock>;
  }

  const onToggleEditCoverMode = (e) => {
    setEditCoverMode(e);
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const userCoverUpdated = async (action, data) => {
    if (data && data.canEdit) {
      return await action({ variables: { criterias: data } })
        .then(async () => {
          await refetch().then(() => {
            if (sessionContext.relogin) {
              sessionContext.relogin();
            }
          });
        })
        .catch(() => {
          showValidationError(
            "Có lỗi xảy ra",
            "Có lỗi xảy ra khi cập nhật dữ liệu, bạn vui lòng thử lại"
          );
        });
    }
  };

  const fullUserInfo = parseUserInfo(data);

  return (
    <Profile
      isEditCoverMode={isEditCoverMode}
      userId={userId}
      pageNumber={pageNumber}
      baseUrl={_baseUrl}
      onToggleEditCoverMode={onToggleEditCoverMode}
      userCoverUpdated={userCoverUpdated}
      showValidationError={showValidationError}
      pages={pages}
      userInfo={fullUserInfo}
    />
  );
});