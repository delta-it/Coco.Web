import Alerts from "./LangData/Alerts";

export function langExtract(langData, key, lang) {
  return langData && langData[key] && langData[key][lang]
    ? langData[key][lang]
    : null;
}

export function getError(key, lang) {
  return (
    langExtract(Alerts, key, lang) &&
    langExtract(Alerts, "ErrorOccurredTryRefeshInputAgain", lang)
  );
}

export const createArray = (from, to, isInvert) => {
  const arr = [];

  if (!!isInvert) {
    for (let index = to; index >= from; index--) {
      arr.push(index);
    }
  } else {
    for (let index = from; index <= to; index++) {
      arr.push(index);
    }
  }

  return arr;
};

export const generateDate = (data) => {
  const { date, month, year } = data;

  if (date && month && year) {
    return new Date(Number(year), Number(month) - 1, Number(date));
  }
  return null;
};

export const fileToBase64 = (file) => {
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });
};

export const base64toBlob = (dataURI, mineType) => {
  var urlSplitted = dataURI.split(",");
  var byteString = null;
  if (urlSplitted.length > 1) {
    byteString = atob(dataURI.split(",")[1]);
  } else {
    byteString = atob(dataURI);
  }

  var buffers = new ArrayBuffer(byteString.length);
  var unitBuffers = new Uint8Array(buffers);

  for (var i = 0; i < byteString.length; i++) {
    unitBuffers[i] = byteString.charCodeAt(i);
  }

  mineType = mineType ? mineType : "image/jpeg";
  return new Blob([buffers], { type: mineType });
};
