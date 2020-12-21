import { gql } from "@apollo/client";

export const SIGNUP = gql`
  mutation Signup($criterias: SignupModelInput) {
    signup(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const LOGIN = gql`
  mutation login($criterias: LoginModelInput!) {
    login(criterias: $criterias) {
      userInfo {
        displayName
        userIdentityId
      }
      authenticationToken
    }
  }
`;

export const UPDATE_USER_IDENTIFIER = gql`
  mutation UpdateIdentifier($criterias: UserIdentifierUpdateRequestInput) {
    updateIdentifier(criterias: $criterias) {
      lastname
      firstname
      displayName
    }
  }
`;

export const UPDATE_USER_INFO_PER_ITEM = gql`
  mutation UpdateUserInfoItem($criterias: UpdatePerItemModelInput!) {
    updateUserInfoItem(criterias: $criterias) {
      value
      propertyName
    }
  }
`;

export const UPDATE_USER_AVATAR = gql`
  mutation UpdateAvatar($criterias: UserPhotoUpdateRequestInput!) {
    updateAvatar(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const DELETE_USER_AVATAR = gql`
  mutation DeleteAvatar($criterias: PhotoDeleteModelInput!) {
    deleteAvatar(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const UPDATE_USER_COVER = gql`
  mutation UpdateCover($criterias: UserPhotoUpdateRequestInput!) {
    updateCover(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const DELETE_USER_COVER = gql`
  mutation DeleteCover($criterias: PhotoDeleteModelInput!) {
    deleteCover(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
      result
    }
  }
`;

export const UPDATE_USER_PASSWORD = gql`
  mutation UpdatePassword($criterias: UserPasswordUpdateRequestInput!) {
    updatePassword(criterias: $criterias) {
      authenticationToken
      isSucceed
    }
  }
`;

export const FORGOT_PASSWORD = gql`
  mutation ForgotPassword($criterias: ForgotPasswordModelInput!) {
    forgotPassword(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const RESET_PASSWORD = gql`
  mutation($criterias: ResetPasswordModelInput!) {
    resetPassword(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const VALIDATE_IMAGE_URL = gql`
  mutation($criterias: ImageValidationModelInput!) {
    validateImageUrl(criterias: $criterias) {
      isSucceed
    }
  }
`;

export const FILTER_ARTICLE_CATEGORIES = gql`
  mutation($criterias: SelectFilterModelInput) {
    categories: articleCategories(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const FILTER_PRODUCT_CATEGORIES = gql`
  mutation($criterias: SelectFilterModelInput) {
    categories: productCategories(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const FILTER_FARM_TYPES = gql`
  mutation($criterias: SelectFilterModelInput) {
    categories: farmTypes(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const FILTER_FARMS = gql`
  mutation($criterias: SelectFilterModelInput) {
    userFarms: selectUserFarms(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const CREATE_ARTICLE = gql`
  mutation($criterias: ArticleModelInput!) {
    createArticle(criterias: $criterias) {
      id
    }
  }
`;

export const UPDATE_ARTICLE = gql`
  mutation($criterias: ArticleModelInput!) {
    updateArticle(criterias: $criterias) {
      id
    }
  }
`;

export const CREATE_PRODUCT = gql`
  mutation($criterias: ProductModelInput!) {
    createProduct(criterias: $criterias) {
      id
    }
  }
`;

export const CREATE_FARM = gql`
  mutation($criterias: FarmModelInput!) {
    createFarm(criterias: $criterias) {
      id
    }
  }
`;

export const UPDATE_FARM = gql`
  mutation($criterias: FarmModelInput!) {
    updateFarm(criterias: $criterias) {
      id
    }
  }
`;

export const UPDATE_PRODUCT = gql`
  mutation($criterias: ProductModelInput!) {
    updateProduct(criterias: $criterias) {
      id
    }
  }
`;
