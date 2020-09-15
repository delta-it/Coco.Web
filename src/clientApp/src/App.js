import React from "react";
import { BrowserRouter, Switch } from "react-router-dom";
import appRoutes from "./routes/AppRoutes";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { library } from "@fortawesome/fontawesome-svg-core";
import loadable from "@loadable/component";
import { ApolloProvider } from "@apollo/client";
import { graphqlClient } from "./utils/GraphQLClient";
import configureModalStore from "./store/hook-store/modal-store";
import configureAvatarStore from "./store/hook-store/avatar-store";
import configureNotifyStore from "./store/hook-store/notify-store";
import { useQuery } from "@apollo/client";
import { GET_LOGGED_USER } from "./utils/GraphQLQueries/queries";
import { SessionContext } from "./store/context/SessionContext";
import AuthService from "./services/AuthService";
import { getLocalStorageByKey } from "./services/StorageService";
import { AUTH_LOGIN_KEY } from "./utils/AppSettings";

configureModalStore();
configureAvatarStore();
configureNotifyStore();

// Font Awesome
const AsyncPage = loadable((props) => import(`${props.page}`));
library.add(fas);

export default () => {
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  const { loading, data, refetch, error } = useQuery(GET_LOGGED_USER, {
    client: graphqlClient,
  });

  const relogin = () => {
    if (refetch) {
      return refetch();
    }
  };

  const parseLoggedUser = (response) => {
    const userInfo = AuthService.parseUserInfo(response);

    if (error) {
      return {};
    }

    return {
      lang: "vn",
      authenticationToken: userInfo.tokenkey,
      isLogin: userInfo.isLogin,
      user: userInfo,
    };
  };

  const userObj = !!isLogin ? parseLoggedUser(data) : {};

  return (
    <ApolloProvider client={graphqlClient}>
      <SessionContext.Provider
        value={{ ...userObj, relogin: relogin, isLoading: loading }}
      >
        <BrowserRouter>
          <Switch>
            $
            {appRoutes.map((route) => {
              var { layout: ComponentLayout, exact, path, page } = route;
              return (
                <ComponentLayout
                  exact={exact}
                  path={path}
                  component={() => <AsyncPage page={`./pages/${page}`} />}
                />
              );
            })}
          </Switch>
        </BrowserRouter>
      </SessionContext.Provider>
    </ApolloProvider>
  );
};
