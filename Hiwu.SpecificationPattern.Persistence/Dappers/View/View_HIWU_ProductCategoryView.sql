-- View: public.View_HIWU_ProductCategoryView

-- DROP VIEW public."View_HIWU_ProductCategoryView";

CREATE OR REPLACE VIEW public."View_HIWU_ProductCategoryView"
 AS
 SELECT p."Id" AS "ProductId",
    p."Name" AS "ProductName",
    p."Price",
    p."Content",
    p."UrlImage",
    c."Id" AS "CategoryId",
    c."Name" AS "CategoryName"
   FROM "Products" p
     LEFT JOIN "Categories" c ON p."CategoryId" = c."Id";

ALTER TABLE public."View_HIWU_ProductCategoryView"
    OWNER TO postgres;

