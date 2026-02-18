const httpPost = async (url, obj) => {
  if (!obj) return null;

  const response = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      withCredentials: true
    },
    body: JSON.stringify(obj),
  });

  return await response.json();
};

const httpGet = async (url) => {
  const response = await fetch(url, {
    method: "GET",
    headers: {
      withCredentials: true
    },
  });

  return response;
};

export { httpPost, httpGet };
