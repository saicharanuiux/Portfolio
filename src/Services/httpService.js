



    const httpPost = async (url, obj) =>{
        if (!obj) return null;

        const response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(obj),
        });

        return await response.json(); 
   }
   
 export {httpPost};